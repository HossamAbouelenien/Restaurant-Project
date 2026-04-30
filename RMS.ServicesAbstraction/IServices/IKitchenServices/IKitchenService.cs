using RMS.Shared.DTOs.KitchenDTOs;
using RMS.Shared.QueryParams;

namespace RMS.ServicesAbstraction.IServices.IKitchenServices
{
    public interface IKitchenService
    {
        Task<KitchenBoardDto> GetAllKitchenTicketsGroupedByStatusForCurrentBranchAsync(KitchenTicketQueryParams queryParams);
        Task<KitchenTicketDetailsDto> GetSingleKitchenTicketWithsOrderItemsAsync(int id);
        Task<List<ActivePendingStationsDTOs>> GetListOfActiveStationsWithPendingCountAsync(int branchId);
        Task<KitchenTicketStatusDto> UpdateTicketStatusAsync(int ticketId, UpdateTicketStatusRequestDto dto);

       Task<bool> UpdateCofirmServeredColumn(int id);

    }
}
