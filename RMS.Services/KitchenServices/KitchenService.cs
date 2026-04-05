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
            var data = _mapper.Map<List<KitchenTicketDTO>>(kitchenTickets);
            var grouped = data
                .GroupBy(t => t.Status)
                .ToDictionary(g => g.Key, g => g.ToList());
            return new KitchenBoardDto
            {
                Pending = grouped.GetValueOrDefault(TicketStatus.Pending, new List<KitchenTicketDTO>()),
                Preparing = grouped.GetValueOrDefault(TicketStatus.Preparing, new List<KitchenTicketDTO>()),
                Done = grouped.GetValueOrDefault(TicketStatus.Done, new List<KitchenTicketDTO>())
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



        public async Task<KitchenTicketStatusDto> UpdateTicketStatusAsync(int ticketId,UpdateTicketStatusRequestDto dto)
        {
            var repo = _unitOfWork.GetRepository<KitchenTicket>();

            var spec = new KitchenTicketWithOrderSpecification(ticketId);
            var ticket = await repo.GetByIdAsync(spec);

            if (ticket == null)
                throw new Exception("Ticket not found");

            // Validate transitions
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

                await DecrementStock(ticket);
                await CheckOrderCompletion(ticket.OrderId);
            }
            else
            {
                throw new Exception("Invalid status");
            }

            await _unitOfWork.SaveChangesAsync();

            // reload
            var updatedTicket = await repo.GetByIdAsync(spec);

            var dtoResult = _mapper.Map<KitchenTicketStatusDto>(updatedTicket);

            // calculate IsOrderReady
            var ticketRepo = _unitOfWork.GetRepository<KitchenTicket>();
            var ticketsSpec = new TicketByOrderSpecification(ticket.OrderId);
            var tickets = await ticketRepo.GetAllAsync(ticketsSpec);

            dtoResult.IsOrderReady = tickets.All(t => t.Status == TicketStatus.Done);

            return dtoResult;
        }


        private async Task DecrementStock(KitchenTicket ticket)
        {
            var orderRepo = _unitOfWork.GetRepository<Order>();
            var stockRepo = _unitOfWork.GetRepository<BranchStock>();
            var spec = new OrderWithItemsAndRecipesSpecification(ticket.OrderId);

            var order = await orderRepo.GetByIdAsync(spec);

            var ingredientIds = order!.OrderItems
                .SelectMany(i => i.MenuItem!.Recipes)
                .Select(r => r.IngredientId)
                .Distinct()
                .ToList();

            var specStock = new StockByBranchAndIngredientsSpecification(order.BranchId, ingredientIds);

            var stocks = await stockRepo.GetAllAsync(specStock);

            var stockDict = stocks.ToDictionary(s => s.IngredientId);

            foreach (var item in order.OrderItems)
            {
                foreach (var recipe in item.MenuItem!.Recipes)
                {
                    if (!stockDict.TryGetValue(recipe.IngredientId, out var stock))
                        throw new Exception($"Ingredient {recipe.IngredientId} not found");

                    var required = recipe.QuantityRequired * item.Quantity;

                    if (stock.QuantityAvailable < required)
                        throw new Exception("Not enough stock");

                    stock.QuantityAvailable -= required;
                }
            }
        }


        private async Task CheckOrderCompletion(int orderId)
        {
            var ticketRepo = _unitOfWork.GetRepository<KitchenTicket>();
            var orderRepo = _unitOfWork.GetRepository<Order>();

            var spec = new TicketByOrderSpecification(orderId);
            var tickets = await ticketRepo.GetAllAsync(spec);

            if (tickets.All(t => t.Status == TicketStatus.Done))
            {
                var order = await orderRepo.GetByIdAsync(orderId);
                order.Status = OrderStatus.Ready;
            }
        }
    }
}

