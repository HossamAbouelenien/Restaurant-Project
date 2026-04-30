using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.Services.Exceptions;
using RMS.Services.Specifications.KitchenTicketSpec;
using RMS.ServicesAbstraction.IServices.IHubServices.IRestaurantNotifier;
using RMS.ServicesAbstraction.IServices.IKitchenServices;
using RMS.Shared.DTOs.KitchenDTOs;
using RMS.Shared.QueryParams;

namespace RMS.Services.Services.KitchenServices
{
    public class KitchenService : IKitchenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRestaurantNotifier _restaurantNotifier;

        public KitchenService(IUnitOfWork unitOfWork, IMapper mapper, IRestaurantNotifier restaurantNotifier)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _restaurantNotifier = restaurantNotifier;
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

                if (kitchenTicket is null)
                {
                     throw new KitchenTicketNotFoundException(id);
                } 

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
                throw new KitchenTicketNotFoundException(ticketId);


            if (dto.Status == TicketStatus.Preparing)
            {

                if (ticket.Status != TicketStatus.Pending)
                {
                    throw new InvalidStatusTransitionException(
                        ticket.Status.ToString(),
                        TicketStatus.Preparing.ToString());
                }
                  

                ticket.Status = TicketStatus.Preparing;
                ticket.StartedAt = DateTime.UtcNow;

            }

            else if (dto.Status == TicketStatus.Done)
            {
                if (ticket.Status != TicketStatus.Preparing)
                {
                    throw new InvalidStatusTransitionException(
                        ticket.Status.ToString(),
                        TicketStatus.Done.ToString());
                }
                   

                ticket.Status = TicketStatus.Done;
                ticket.CompletedAt = DateTime.UtcNow;

                //await DecrementStock(ticket);
            }

            else
            {
                throw new InvalidStatusValueException(dto.Status.ToString());
            }

           
            await UpdateOrderStatus(ticket.OrderId);

            await _unitOfWork.SaveChangesAsync();

            
            var updatedTicket = await repo.GetByIdAsync(spec);

            var dtoResult = _mapper.Map<KitchenTicketStatusDto>(updatedTicket);

            var ticketRepo = _unitOfWork.GetRepository<KitchenTicket>();
            var tickets = await ticketRepo.GetAllAsync(new TicketByOrderSpecification(ticket.OrderId));

            dtoResult.IsOrderReady = tickets.All(t => t.Status == TicketStatus.Done);

            await _restaurantNotifier.SendAsync(
                     "KitchenUpdated",
                     dtoResult,
                     $"kitchen_branch_{ticket.Order!.BranchId}",
                     "admins"
                    );

            return dtoResult;
        }




        public async Task<bool> UpdateCofirmServeredColumn(int id)
        {
            var repo = _unitOfWork.GetRepository<KitchenTicket>();
            var spec = new KitchenTicketWithOrderSpecification(id);
            var kitchenTicket = await repo.GetByIdAsync(spec);

            if (kitchenTicket == null)
            {
                throw new KitchenTicketNotFoundException(id);
            }
                

            kitchenTicket.ConfirmedServed = true;

            await _unitOfWork.SaveChangesAsync();

            await _restaurantNotifier.SendAsync(
                     "CofirmServeredUpdated",
                     kitchenTicket,
                     $"waiters_branch_{kitchenTicket.Order!.BranchId}"
                    );

            return true;

        }




        private async Task UpdateOrderStatus(int orderId)
        {
            var ticketRepo = _unitOfWork.GetRepository<KitchenTicket>();
            var orderRepo = _unitOfWork.GetRepository<Order>();

            var tickets = await ticketRepo.GetAllAsync(new TicketByOrderSpecification(orderId));

            var order = await orderRepo.GetByIdAsync(orderId);

            if (order == null)
            {
                throw new OrderNotFoundException(orderId);
            }
              

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
    }


















}











































