using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.Services.Specifications.BranchStockSpec;
using RMS.Services.Specifications.KitchenTicketSpec;
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
            // Parse PaymentMethod
            if (!Enum.TryParse<PaymentMethod>(orderDto.PaymentMethod, ignoreCase: true, out var paymentMethod))
                throw new Exception($"Invalid PaymentMethod: {orderDto.PaymentMethod}");
            var Repo = _unitOfWork.GetRepository<Order>();
            var ItemRepo = _unitOfWork.GetRepository<MenuItem>();
            var orderItems = new List<CreateOrderItemDTO>();
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
                orderItems.Add(new CreateOrderItemDTO { MenuItemId = menuItem.Id, Quantity = item.Quantity, UnitPrice = menuItem.Price, Notes=item.Notes}); 
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
                                var stock = r.Ingredient!.BranchStocks
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

            order.Payment = new Payment
            {
                PaymentMethod = paymentMethod,
                PaymentStatus = PaymentStatus.Pending
            };

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
                var categories = new HashSet<string>();

                foreach (var item in order.OrderItems)
                {
                    var menuItemSpec = new MenuItemWithBranchStockSpecification(item.MenuItemId);
                    var menuItem = await ItemRepo.GetByIdAsync(menuItemSpec) ?? throw new Exception("Menu item not found");
                    if (categories.Add(menuItem.Category!.Name))
                    {
                        var KichenTicketRepo = _unitOfWork.GetRepository<KitchenTicket>();
                        await KichenTicketRepo.AddAsync(new KitchenTicket
                        {
                            OrderId = order.Id,
                            Station = menuItem.Category.Name
                        });
                    }
                }
                await _unitOfWork.SaveChangesAsync();
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

        public async Task<OrderDTO> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var orderRepo = _unitOfWork.GetRepository<Order>();
            var orderSpec = new OrderWithTableOrderAndBranchAndCustomerAndOrderItemsSpecification(orderId);
            var orderToUpdate = await orderRepo.GetByIdAsync(orderSpec);
            if (orderToUpdate == null) throw new Exception("Order not found");
            if (Enum.TryParse(typeof(OrderStatus), newStatus, true, out var parsedStatus))
            {
                switch (orderToUpdate.Status)
                {
                    case OrderStatus.Received:
                        if (parsedStatus is OrderStatus.Preparing or OrderStatus.Cancelled)
                            orderToUpdate.Status = (OrderStatus)parsedStatus;
                        else
                            throw new Exception("Invalid status transition from Received");
                        break;
                    case OrderStatus.Preparing:
                        if (parsedStatus is OrderStatus.Ready)
                            orderToUpdate.Status = (OrderStatus)parsedStatus;
                        else
                            throw new Exception("Invalid status transition from Preparing");
                        break;
                    case OrderStatus.Ready:
                        if (parsedStatus is OrderStatus.Delivered)
                            orderToUpdate.Status = (OrderStatus)parsedStatus;
                        else
                            throw new Exception("Invalid status transition from Ready");
                        break;
                    case OrderStatus.Delivered:
                    case OrderStatus.Cancelled:
                        throw new Exception("Cannot change status of Delivered or Cancelled orders");
                    default:
                        throw new Exception("Unknown current order status");
                }
                // If order is being marked as Delivered, set DeliveryStatus to Delivered if it's a Delivery order
                if (orderToUpdate.Status == OrderStatus.Delivered && orderToUpdate.OrderType == OrderType.Delivery)
                {
                    if (orderToUpdate.Delivery != null)
                    {
                        orderToUpdate.Delivery.DeliveryStatus = DeliveryStatus.Delivered;
                        orderToUpdate.Delivery.UpdatedAt = DateTime.Now;
                    }
                    if (orderToUpdate.OrderType == OrderType.DineIn)
                    {
                        var table = await _unitOfWork.GetRepository<Table>().GetByIdAsync(orderToUpdate.TableOrder!.TableId);
                        if (table != null)
                        {
                            table.IsOccupied = false;
                        }
                    }
                }
                // If order is being cancelled, release reserved stock and free up table if DineIn
                if (orderToUpdate.Status == OrderStatus.Cancelled)
                {
                    orderToUpdate.TotalAmount = 0;
                    // Free up table if DineIn
                    if (orderToUpdate.OrderType == OrderType.DineIn)
                    {
                        var table = await _unitOfWork.GetRepository<Table>().GetByIdAsync(orderToUpdate.TableOrder!.TableId);
                        if (table != null)
                        {
                            table.IsOccupied = false;
                        }
                    }
                    var kitchenTicketSpec = new TicketByOrderSpecification(orderToUpdate.Id);
                    var tickets = await _unitOfWork.GetRepository<KitchenTicket>().GetAllAsync(kitchenTicketSpec);
                    foreach (var ticket in tickets)
                    {
                        ticket.IsDeleted = true;
                        ticket.DeletedAt = DateTime.Now;
                    }
                    // Real Time Kitchen notification to cancel tickets would be implemented here (e.g., via SignalR)
                    if (orderToUpdate.Delivery is not null)
                    {
                        var delivery = await _unitOfWork.GetRepository<Delivery>().GetByIdAsync(orderToUpdate.Delivery.Id);
                        if (delivery is not null)
                        {
                            delivery.IsDeleted = true;
                            delivery.DeletedAt = DateTime.Now;
                        }   
                    }
                    if(orderToUpdate.Payment is not null)
                    {
                        var payment = await _unitOfWork.GetRepository<Payment>().GetByIdAsync(orderToUpdate.Payment.Id);
                        if (payment is not null && payment.PaymentStatus == PaymentStatus.Paid)
                        {
                            payment.PaymentStatus = PaymentStatus.Refunded;
                            payment.UpdatedAt = DateTime.Now;
                        }
                    }

                }
            }
            else
            {
                throw new Exception("Invalid order status");
            }
            orderToUpdate.UpdatedAt = DateTime.Now;
            var updatedResult = await _unitOfWork.SaveChangesAsync();
            if (updatedResult > 0)
            {
                return _mapper.Map<OrderDTO>(orderToUpdate);
            }
            else
            {
                throw new Exception("Failed to update order status");
            }
        }

        public async Task<AddedItemsDTO> AddItemsToOrderAsync(int orderId, List<CreateOrderItemDTO> items)
        {
            var orderRepo = _unitOfWork.GetRepository<Order>();
            var orderSpec = new OrderWithTableOrderAndBranchAndCustomerAndOrderItemsSpecification(orderId);
            var order = await orderRepo.GetByIdAsync(orderSpec);
            if (order == null) throw new Exception("Order not found");
            if (order.Status != OrderStatus.Received)
                throw new Exception("Can only add items to orders with 'Received' status");

            var ItemRepo = _unitOfWork.GetRepository<MenuItem>();


            var OldIngredientConsumption = new Dictionary<int, decimal>();
            var NewIngredientConsumption = new Dictionary<int, decimal>();

            foreach (var item in order.OrderItems)
            {
                var menuItemSpec = new MenuItemWithBranchStockSpecification(item.MenuItemId);
                var menuItem = await ItemRepo.GetByIdAsync(menuItemSpec) ?? throw new Exception("Menu item not found");
                foreach (var recipe in menuItem.Recipes)
                {
                    var totalRequired = recipe.QuantityRequired * item.Quantity;
                    // Accumulate total required quantity for each ingredient across all items
                    if (OldIngredientConsumption.ContainsKey(recipe.IngredientId))
                        OldIngredientConsumption[recipe.IngredientId] += totalRequired;
                    else
                        OldIngredientConsumption[recipe.IngredientId] = totalRequired;
                }
            }
            foreach (var item in items)
            {
                var menuItemSpec = new MenuItemWithBranchStockSpecification(item.MenuItemId);
                var menuItem = await ItemRepo.GetByIdAsync(menuItemSpec) ?? throw new Exception("Menu item not found");
                foreach (var recipe in menuItem.Recipes)
                {
                    var totalRequired = recipe.QuantityRequired * item.Quantity;
                    // Accumulate total required quantity for each ingredient across all items
                    if (NewIngredientConsumption.ContainsKey(recipe.IngredientId))
                        NewIngredientConsumption[recipe.IngredientId] += totalRequired;
                    else
                        NewIngredientConsumption[recipe.IngredientId] = totalRequired;
                }
            }


            foreach (var (ingredientId, totalRequired) in NewIngredientConsumption)
            {
                var StockSpec = new BranchStockWithBranchAndIngredient(order.BranchId, ingredientId);
                var stock = await _unitOfWork.GetRepository<BranchStock>().GetAllAsync(StockSpec);
                var stockItem = stock.FirstOrDefault();
                if (stockItem == null)
                    throw new Exception($"Ingredient ID {ingredientId} not found in branch");

                var remainingStock = stockItem.QuantityAvailable - OldIngredientConsumption.GetValueOrDefault(ingredientId, 0);

                if (remainingStock < totalRequired)
                {
                    // Build detailed error message showing how many of each menu item can be ordered based on the limiting ingredient
                    string ErrorMessage = string.Empty;
                    // For each ordered menu item, calculate how many can be made with the available stock of the limiting ingredient
                    foreach (var item in items)
                    {
                        var menuItemSpec = new MenuItemWithBranchStockSpecification(item.MenuItemId);
                        var menuItem = await ItemRepo.GetByIdAsync(menuItemSpec)
                            ?? throw new Exception("Menu item not found");

                        // For this menu item, find the recipe that uses the limiting ingredient and calculate how many can be made
                        var limitingIngredient = menuItem.Recipes
                            .Select(r =>
                            {
                                var stock = r.Ingredient!.BranchStocks
                                    .FirstOrDefault(bs => bs.BranchId == order.BranchId);

                                var available = stock?.QuantityAvailable - OldIngredientConsumption.GetValueOrDefault(ingredientId, 0) ?? 0;
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

            var categories = new HashSet<string>();

            foreach (var item in order.OrderItems)
            {
                var menuItemSpec = new MenuItemWithBranchStockSpecification(item.MenuItemId);
                var menuItem = await ItemRepo.GetByIdAsync(menuItemSpec) ?? throw new Exception("Menu item not found");
                categories.Add(menuItem.Category!.Name);
            }

            foreach (var item in items)
            {
                var menuItemSpec = new MenuItemWithBranchStockSpecification(item.MenuItemId);
                var menuItem = await ItemRepo.GetByIdAsync(menuItemSpec) ?? throw new Exception("Menu item not found");
                // check if item already exists in order, if so, update quantity and unit price
                var existingItem = order.OrderItems.FirstOrDefault(oi => oi.MenuItemId == item.MenuItemId);
                item.UnitPrice = menuItem.Price;

                if (existingItem != null)
                {
                    existingItem.Quantity += item.Quantity;
                    order.TotalAmount += item.Quantity * menuItem.Price;
                    continue; // Skip adding a new item
                }
                order.OrderItems.Add(new OrderItem
                {
                    MenuItemId = item.MenuItemId,
                    Quantity = item.Quantity,
                    UnitPrice = menuItem.Price,
                    Notes = item.Notes
                });
                    order.TotalAmount += item.Quantity * menuItem.Price ;
            }

            var addedItemsDto = new AddedItemsDTO
            {
                OrderID = order.Id,
                AddedItems = items
            };

            var result = await _unitOfWork.SaveChangesAsync();
            if (result > 0)
            {
                
                
                foreach (var item in items)
                {
                    var menuItemSpec = new MenuItemWithBranchStockSpecification(item.MenuItemId);
                    var menuItem = await ItemRepo.GetByIdAsync(menuItemSpec) ?? throw new Exception("Menu item not found");
                    if (categories.Add(menuItem.Category!.Name))
                    {
                        var kitchenTicketRepo = _unitOfWork.GetRepository<KitchenTicket>();
                        await kitchenTicketRepo.AddAsync(new KitchenTicket
                        {
                            OrderId = order.Id,
                            Station = menuItem.Category.Name
                        });
                    }
                }
                await _unitOfWork.SaveChangesAsync();
                return addedItemsDto;
            }
            else
            {
                throw new Exception("Failed to add items to order");
            }
        }

        public async Task<OrderDTO> RemoveItemsFromOrderAsync(int orderId, int itemId)
        {
            var repo = _unitOfWork.GetRepository<Order>();
            var spec = new OrderWithTableOrderAndBranchAndCustomerAndOrderItemsSpecification(orderId);
            var order = await repo.GetByIdAsync(spec);
            var menuItemRepo = _unitOfWork.GetRepository<MenuItem>();
            if (order is null) throw new Exception("Order not found");
            if (order.Status != OrderStatus.Received) throw new Exception("Order is not in a state that allows item removal");
            var orderItemSpec = new OrderItemWithCategorySpecification(itemId);
            var orderItemRepo = _unitOfWork.GetRepository<OrderItem>();
            var orderItem = await orderItemRepo.GetByIdAsync(orderItemSpec);           
            if(orderItem is null || orderItem.OrderId != orderId) throw new Exception("Order item not found in order");
            order.OrderItems.Remove(orderItem);
            if(!order.OrderItems.Any()) throw new Exception("Cannot remove all items from order. Consider cancelling the order instead.");
            var newCategories = new HashSet<string>();
            foreach (var item in order.OrderItems)
            {
                var menuItemSpec = new MenuItemWithBranchStockSpecification(item.MenuItemId);
                var menuItem = await menuItemRepo.GetByIdAsync(menuItemSpec) ?? throw new Exception("Menu item not found");
                newCategories.Add(menuItem.Category!.Name);
            }
            if (!newCategories.Contains(orderItem.MenuItem!.Category!.Name))
            {
                var kitchenTicketRepo = _unitOfWork.GetRepository<KitchenTicket>();
                var ticketSpec = new TicketByOrderSpecification(order.Id);
                var tickets = await kitchenTicketRepo.GetAllAsync(ticketSpec);
                var ticketToRemove = tickets.FirstOrDefault(t => t.Station == orderItem.MenuItem.Category!.Name);
                if (ticketToRemove != null)
                {
                     kitchenTicketRepo.Remove(ticketToRemove);
                }
            }

            order.TotalAmount -= orderItem.Quantity * orderItem.UnitPrice;
            order.UpdatedAt = DateTime.Now;
            var result = await _unitOfWork.SaveChangesAsync();
            if (result > 0)
                return _mapper.Map<OrderDTO>(order);
            else
                throw new Exception("Failed to remove items from order");
        }

        public async Task CancelOrderAsync(int orderId)
        {
            
            // Get order to cancel
            var orderRepo = _unitOfWork.GetRepository<Order>();
            var orderSpec = new OrderWithTableOrderAndBranchAndCustomerAndOrderItemsSpecification(orderId);
            var orderToCancel = await orderRepo.GetByIdAsync(orderSpec);
            if (orderToCancel == null) throw new Exception("Order not found");
            if (orderToCancel.Status != OrderStatus.Received) throw new Exception("Order is not in a state that allows cancellation");
            // when cancelling an order, we need to:
            // 1. Set order status to Cancelled
            orderToCancel.Status = OrderStatus.Cancelled;
            // 2. If DineIn, free up the table
            if (orderToCancel.OrderType == OrderType.DineIn)
            {
                var table = await _unitOfWork.GetRepository<Table>().GetByIdAsync(orderToCancel.TableOrder!.TableId);
                if (table != null)
                {
                    table.IsOccupied = false;
                }
            }
            // 3. Mark related kitchen tickets as cancelled (or deleted)
            var kitchenTicketSpec = new TicketByOrderSpecification(orderToCancel.Id);
            var tickets = await _unitOfWork.GetRepository<KitchenTicket>().GetAllAsync(kitchenTicketSpec);
            foreach (var ticket in tickets)
            {
                ticket.IsDeleted = true;
                ticket.DeletedAt = DateTime.Now;
            }
            // 4. If Delivery, mark delivery as cancelled (or deleted)
            if (orderToCancel.Delivery is not null)
            {
                var delivery = await _unitOfWork.GetRepository<Delivery>().GetByIdAsync(orderToCancel.Delivery.Id);
                if (delivery is not null)
                {
                    delivery.IsDeleted = true;
                    delivery.DeletedAt = DateTime.Now;
                }
            }
            // 5. If payment was already made, mark payment as refunded
            if (orderToCancel.Payment is not null)
            {
                var payment = await _unitOfWork.GetRepository<Payment>().GetByIdAsync(orderToCancel.Payment.Id);
                if (payment is not null && payment.PaymentStatus == PaymentStatus.Paid)
                {
                    payment.PaymentStatus = PaymentStatus.Refunded;
                    payment.UpdatedAt = DateTime.Now;
                }
            }

            orderToCancel.UpdatedAt = DateTime.Now;
            var updatedResult = await _unitOfWork.SaveChangesAsync();
            if (!(updatedResult > 0))
                throw new Exception("Failed to update order status");
            
        }
    }
}
