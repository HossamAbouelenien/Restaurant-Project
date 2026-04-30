using RMS.Shared;
using RMS.Shared.DTOs.DeliveryDTOs;
using RMS.Shared.QueryParams;

namespace RMS.ServicesAbstraction.IServices.IDeliveryServices
{
    public interface IDeliveryService
    {
        Task<PaginatedResult<DeliveryDetailsDto>> GetAllDeliveriesAsync(DeliveryQueryParams queryParams);
        Task<IEnumerable<DeliveryDetailsDto>> GetOwnAssignedDeliveriesAsync();
        Task<DeliveryDetailsDto> GetDeliveryByIdAsync(int id);
        Task<DeliveryDetailsDto> AssignDriverAsync(AssignDeliveryDto dto);
        Task<DeliveryDetailsDto> UpdateDeliveryStatusAsync(int id, UpdateDeliveryStatusDto dto, string userId, bool isAdmin);
        Task<List<UnAssignDeliveryDto>> GetUnAssignedDeliveriesAsync();
        Task<PaginatedResult<AvailableDriverDto>> GetAvailableDriversAsync(AvailableDriversQueryParams query);

    }
}
