using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.Services.Specifications.BranchStockSpec;
using RMS.Services.Specifications.KitchenTicketSpec;
using RMS.Services.Specifications.StockSpec;
using RMS.ServicesAbstraction;
using RMS.ServicesAbstraction.IKitchenServices;
using RMS.Shared.DTOs.BranchStockDTOs;
using RMS.Shared.DTOs.KitchenDTOs;
using RMS.Shared.QueryParams;

namespace RMS.Services.KitchenServices
{
    public class KitchenService : IKitchenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public KitchenService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<KitchenBoardDto> GetAllKitchenTicketsGroupedByStatusForCurrentBranchAsync(KitchenTicketQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<KitchenTicket>();
            var spec = new KitchenTicketsGroupedByStatusForCurrentBranchSpecification(queryParams);
            var kitchenTickets = await repo.GetAllAsync(spec);
            var data = _mapper.Map<List<OrderKitchenTicketDTO>>(kitchenTickets);
            var grouped = data
                .GroupBy(t => t.Status)
                .ToDictionary(g => g.Key, g => g.ToList());
            return new KitchenBoardDto
            {
                Pending = grouped.GetValueOrDefault(TicketStatus.Pending, new List<OrderKitchenTicketDTO>()),
                Preparing = grouped.GetValueOrDefault(TicketStatus.Preparing, new List<OrderKitchenTicketDTO>()),
                Done = grouped.GetValueOrDefault(TicketStatus.Done, new List<OrderKitchenTicketDTO>())
            };
        }

        public async Task<KitchenTicketDetailsDto> GetSingleKitchenTicketWithsOrderItemsAsync(int id)
        {
                var Repo = _unitOfWork.GetRepository<KitchenTicket>();
                var Spec = new KitchenTicketsGroupedByStatusForCurrentBranchSpecification(id);
                var kitchenTicket = await Repo.GetByIdAsync(Spec);
                var DataToReturn = _mapper.Map<KitchenTicketDetailsDto>(kitchenTicket);
                return DataToReturn;
        }


        public async Task<List<ActivePendingStationsDTOs>> GetListOfActiveStationsWithPendingCountAsync(int branchId)
        {
            var repo = _unitOfWork.GetRepository<KitchenTicket>();
            var spec = new ActiveStationsForBranchWithPendingTicketCountSpecification(branchId);
            var tickets = await repo.GetAllAsync(spec);
            var result = tickets
                .GroupBy(t => t.Station)                 
                .Select(g => new ActivePendingStationsDTOs
                {
                    Station = g.Key,
                    PendingCount = g.Count()
                })
                .ToList();

            return result;
        }


        public async Task<KitchenTicketStatusDto> UpdateTicketStatusAsync(int ticketId, UpdateTicketStatusRequestDto dto)
        {
            var repo = _unitOfWork.GetRepository<KitchenTicket>();

            var spec = new KitchenTicketWithOrderSpecification(ticketId);
            var ticket = await repo.GetByIdAsync(spec);

            if (ticket == null)
                throw new Exception("Ticket not found");

           
            if (dto.Status == TicketStatus.Preparing)
            {
                if (ticket.Status != TicketStatus.Pending)
                    throw new Exception("Invalid transition");

                ticket.Status = TicketStatus.Preparing;
                ticket.StartedAt = DateTime.UtcNow;
            }
            else if (dto.Status == TicketStatus.Done)
            {
                if (ticket.Status != TicketStatus.Preparing)
                    throw new Exception("Invalid transition");

                ticket.Status = TicketStatus.Done;
                ticket.CompletedAt = DateTime.UtcNow;

                //await DecrementStock(ticket);
            }
            else
            {
                throw new Exception("Invalid status");
            }

           
            await UpdateOrderStatus(ticket.OrderId);

            await _unitOfWork.SaveChangesAsync();

            
            var updatedTicket = await repo.GetByIdAsync(spec);

            var dtoResult = _mapper.Map<KitchenTicketStatusDto>(updatedTicket);

            var ticketRepo = _unitOfWork.GetRepository<KitchenTicket>();
            var tickets = await ticketRepo.GetAllAsync(new TicketByOrderSpecification(ticket.OrderId));

            dtoResult.IsOrderReady = tickets.All(t => t.Status == TicketStatus.Done);

            return dtoResult;
        }

        public async Task<bool> UpdateCofirmServeredColumn(int id)
        {
            var repo = _unitOfWork.GetRepository<KitchenTicket>();

            var kitchenTicket = await repo.GetByIdAsync(id);

            if (kitchenTicket == null)
                throw new Exception("Ticket not found");

            kitchenTicket.ConfirmedServed = true;

            await _unitOfWork.SaveChangesAsync();

            return true;

        }

        private async Task UpdateOrderStatus(int orderId)
        {
            var ticketRepo = _unitOfWork.GetRepository<KitchenTicket>();
            var orderRepo = _unitOfWork.GetRepository<Order>();

            var tickets = await ticketRepo.GetAllAsync(new TicketByOrderSpecification(orderId));

            var order = await orderRepo.GetByIdAsync(orderId);

            if (order == null)
                throw new Exception("Order not found");

            if (tickets.All(t => t.Status == TicketStatus.Done))
            {
                order.Status = OrderStatus.Ready;
            }
            else if (tickets.Any(t => t.Status == TicketStatus.Preparing))
            {
                order.Status = OrderStatus.Preparing;
            }
            else
            {
                order.Status = OrderStatus.Received;
            }
        }


        //private async Task DecrementStock(KitchenTicket ticket)
        //{
        //    var orderRepo = _unitOfWork.GetRepository<Order>();
        //    var stockRepo = _unitOfWork.GetRepository<BranchStock>();

        //    var spec = new OrderWithItemsAndRecipesSpecification(ticket.OrderId);
        //    var order = await orderRepo.GetByIdAsync(spec);

        //    var branchId = order!.BranchId;

        //    var ingredientIds = order.OrderItems
        //        .SelectMany(i => i.MenuItem!.Recipes)
        //        .Select(r => r.IngredientId)
        //        .Distinct()
        //        .ToList();

        //    var stocks = await stockRepo.GetAllAsync(
        //        new StockByBranchAndIngredientsSpecification(branchId, ingredientIds)
        //    );

        //    var stockDict = stocks.ToDictionary(s => s.IngredientId);

        //    foreach (var item in order.OrderItems)
        //    {
        //        foreach (var recipe in item.MenuItem!.Recipes)
        //        {
        //            var ingredientId = recipe.IngredientId;
        //            var ingredientName = recipe.Ingredient?.Name ?? "Unknown";

                   
        //            if (!stockDict.TryGetValue(ingredientId, out var stock))
        //            {
        //                throw new Exception(
        //                    $"❌ Ingredient not found → Branch: {branchId} | Id: {ingredientId} | Name: {ingredientName}"
        //                );
        //            }

        //            var required = recipe.QuantityRequired * item.Quantity;

                   
        //            if (stock.QuantityAvailable < required)
        //            {
        //                throw new Exception(
        //                    $"❌ Not enough stock → Branch: {branchId} | Ingredient: {ingredientName} (Id: {ingredientId}) | Required: {required} | Available: {stock.QuantityAvailable}"
        //                );
        //            }

                    
        //            stock.QuantityAvailable -= required;

                  
        //            if (stock.QuantityAvailable < stock.LowThreshold)
        //            {
        //                //Notification
        //                Console.WriteLine(
        //                    $"⚠️ Low stock → Branch: {branchId} | Ingredient: {ingredientName} (Id: {ingredientId}) | Remaining: {stock.QuantityAvailable}"
        //                );
        //            }
        //        }
        //    }
        //}

    }
}

