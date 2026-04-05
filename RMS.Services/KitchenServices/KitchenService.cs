using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.Services.Specifications.BranchStockSpec;
using RMS.Services.Specifications.KitchenTicketSpec;
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


        
    }
}
