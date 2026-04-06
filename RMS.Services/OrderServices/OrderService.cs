using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.Services.Specifications.BranchStockSpec;
using RMS.Services.Specifications.MenuItemSpec;
using RMS.Services.Specifications.OrderSpec;
using RMS.ServicesAbstraction;
using RMS.Shared;
using RMS.Shared.DTOs.MenuItemsDTOs;
using RMS.Shared.DTOs.OrderDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<OrderDTO> CreateOrderAsync(CreateOrderDTO orderDto)
        {
            // Parse OrderType
            if (!Enum.TryParse<OrderType>(orderDto.OrderType, ignoreCase: true, out var orderType))
                throw new Exception($"Invalid OrderType: {orderDto.OrderType}");
            var Repo = _unitOfWork.GetRepository<Order>();
            var ItemRepo = _unitOfWork.GetRepository<MenuItem>();
            var orderItems = new List<OrderItemDTO>();
            // Validate OrderType-specific fields
            if (orderType == OrderType.DineIn && !orderDto.TableId.HasValue)
                throw new Exception("TableId is required for DineIn orders");

            if (orderType == OrderType.Delivery && orderDto.DeliveryAddress is null)
                throw new Exception("DeliveryAddress is required for Delivery orders");

            // Validate Items
            if (orderDto.Items == null || !orderDto.Items.Any())
                throw new Exception("Order must have at least one item");

            var duplicateItems = orderDto.Items.GroupBy(i => i.MenuItemId).Where(g => g.Count() > 1);
            if (duplicateItems.Any())
                throw new Exception("Duplicate MenuItems are not allowed");

            // Validate branch and customer existence
            var branchExists = await _unitOfWork.GetRepository<Branch>().GetByIdAsync(orderDto.BranchId) != null;
            var UserSpec = new UserSpecification(orderDto.CustomerId!);
            var customerExists = await _unitOfWork.GetRepository<User>().GetByIdAsync(UserSpec) != null;
            if (!branchExists) throw new Exception("Branch not found");
            if (!customerExists) throw new Exception("Customer not found");

            var ingredientConsumption = new Dictionary<int, decimal>();


            foreach (var item in orderDto.Items)
            {
                var menuItemSpec = new MenuItemWithBranchStockSpecification(item.MenuItemId);
                var menuItem = await ItemRepo.GetByIdAsync(menuItemSpec) ?? throw new Exception("Menu item not found");


                foreach (var recipe in menuItem.Recipes)
                {

                    var totalRequired = recipe.QuantityRequired * item.Quantity;
                    // Accumulate total required quantity for each ingredient across all items
                    if (ingredientConsumption.ContainsKey(recipe.IngredientId))
                        ingredientConsumption[recipe.IngredientId] += totalRequired;
                    else
                        ingredientConsumption[recipe.IngredientId] = totalRequired;
                }
                orderItems.Add(new OrderItemDTO { MenuItemId = menuItem.Id, Quantity = item.Quantity, UnitPrice = menuItem.Price, Notes=item.Notes}); 
            }
            foreach (var (ingredientId, totalRequired) in ingredientConsumption)
            {
                var StockSpec = new BranchStockWithBranchAndIngredient(orderDto.BranchId, ingredientId);
                var stock = await _unitOfWork.GetRepository<BranchStock>().GetAllAsync(StockSpec);

                var stockItem = stock.FirstOrDefault();

                if (stockItem == null)
                    throw new Exception($"Ingredient ID {ingredientId} not found in branch");

                if (stockItem.QuantityAvailable < totalRequired)
                {
                    // Build detailed error message showing how many of each menu item can be ordered based on the limiting ingredient
                    string ErrorMessage = string.Empty;
                    // For each ordered menu item, calculate how many can be made with the available stock of the limiting ingredient
                    foreach (var item in orderDto.Items)
                    {
                        var menuItemSpec = new MenuItemWithBranchStockSpecification(item.MenuItemId);
                        var menuItem = await ItemRepo.GetByIdAsync(menuItemSpec)
                            ?? throw new Exception("Menu item not found");

                        // For this menu item, find the recipe that uses the limiting ingredient and calculate how many can be made
                        var limitingIngredient = menuItem.Recipes
                            .Select(r =>
                            {
                                var stock = r.Ingredient.BranchStocks
                                    .FirstOrDefault(bs => bs.BranchId == orderDto.BranchId);

                                var available = stock?.QuantityAvailable ?? 0;
                                // Calculate how many of this menu item can be made with the available stock of this ingredient
                                return new
                                {
                                    IngredientName = r.Ingredient.Name,
                                    MaxPossible = (int)Math.Floor(available / r.QuantityRequired)
                                };
                            })
                            .OrderBy(x => x.MaxPossible)
                            .First(); // The most limiting ingredient determines how many of this menu item can be made
                        // If this menu item is part of the order, include it in the error message
                        ErrorMessage += $"You can only order {limitingIngredient.MaxPossible} of {menuItem.Name} " +
                                        $"because '{limitingIngredient.IngredientName}' is insufficient.\n";
                    }
                    throw new Exception($"{ErrorMessage}");
                }
            }

            orderDto.Items = orderItems;
            var order = _mapper.Map<Order>(orderDto);                 
            order.TotalAmount = orderItems.Sum(i => i.Quantity * i.UnitPrice);


            //OrderType-specific records
            if (orderType == OrderType.DineIn)
            {
                // Validate Table belongs to same Branch
                var table = await _unitOfWork.GetRepository<Table>().GetByIdAsync(orderDto.TableId!.Value);
                if (table is null)
                    throw new Exception("Table not found");
                if (table.BranchId != orderDto.BranchId)
                    throw new Exception("Table does not belong to this Branch");
                if (table.IsOccupied)
                    throw new Exception("Table is already occupied");

                // Create TableOrder (SeatedAt = CreatedAt handled by EF)
                order.TableOrder = new TableOrder
                {
                    TableId = orderDto.TableId!.Value
                };

                // Mark table as occupied
                table.IsOccupied = true;
            }
            else if (orderType == OrderType.Delivery)
            {
                // Create Delivery record (AssignedAt = CreatedAt, Driver assigned later)
                order.Delivery = new Delivery
                {
                    DeliveryAddress = _mapper.Map<Address>(orderDto.DeliveryAddress),
                    DeliveryStatus = DeliveryStatus.Assigned,
                    //DriverId = "0"  // ← assigned later by Admin
                };
            }


            /////////////////////////////////////////////////////////////////////////////////
            await Repo.AddAsync(order);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result > 0)
            {
                var Spec = new OrderWithBranchAndCustomerAndOrderItemsSpecification(order.Id);
                order = await Repo.GetByIdAsync(Spec) ?? throw new Exception("Failed to retrieve created order");
                return _mapper.Map<OrderDTO>(order);               
            }
            throw new Exception("Failed to create order");
        }

        public async Task<PaginatedResult<OrderDTO>> GetAllOrdersAsync(OrderQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<Order>();
            var spec = new OrderWithBranchAndCustomerAndOrderItemsSpecification(queryParams);
            var orders = await repo.GetAllAsync(spec);
            var orderDtos = _mapper.Map<IEnumerable<OrderDTO>>(orders);
            var countSpec = new OrderCountSpecification(queryParams);
            var countOfOrders = await repo.CountAsync(countSpec);
            var pageSize = orderDtos.Count();
            var paginatedResult = new PaginatedResult<OrderDTO>(queryParams.PageIndex, pageSize, countOfOrders, orderDtos);
            return paginatedResult;
        }

        public async Task<OrderDetailsDTO> GetOrderByIdAsync(int id)

        {
        var repo = _unitOfWork.GetRepository<Order>();
            var spec = new OrderWithItemsAndPaymentAndDeliveryAndKitchenTicketsSpecification(id);
        var order = await repo.GetByIdAsync(spec) ?? throw new Exception("Order not found");
            if (order == null) throw new Exception("order is not found");
            var orderDetailsDto = _mapper.Map<OrderDetailsDTO>(order);

            return orderDetailsDto;
        }



        public async Task<PaginatedResult<OrderDTO>> GetCustomerOrdersHistoryAsync(OrderQueryParams queryParams, string customerId)
        {
            var repo = _unitOfWork.GetRepository<Order>();
            var spec = new OrderWithBranchAndCustomerAndOrderItemsSpecification(queryParams, customerId);
            var orders = await repo.GetAllAsync(spec);
            var orderDtos = _mapper.Map<IEnumerable<OrderDTO>>(orders);
            var countSpec = new OrderCountSpecification(queryParams, customerId);
            var countOfOrders = await repo.CountAsync(countSpec);
            var pageSize = orderDtos.Count();
            var paginatedResult = new PaginatedResult<OrderDTO>(queryParams.PageIndex, pageSize, countOfOrders, orderDtos);
            return paginatedResult;
        }
    }
}
