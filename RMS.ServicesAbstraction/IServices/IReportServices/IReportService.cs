using RMS.Shared.DTOs.ReportsDTOs;

namespace RMS.ServicesAbstraction.IServices.IReportServices
{
    public interface IReportService
    {
        Task<DashboardDTO> GetDashboardAsync();
        Task<IEnumerable<RevenueDTO>> GetRevenueAsync(int? branchId, DateTime? from, DateTime? to);
        Task<OrdersByTypeDTO> GetOrdersByTypeAsync();
        Task<IEnumerable<TopItemsDto>> GetTopItemsAsync(int top);
        Task<IEnumerable<InventoryUsageDTO>> GetInventoryUsageAsync();
        Task<IEnumerable<DailyRevenueDTO>> GetDailyRevenueLast7DaysAsync(int? branchId);
    }
}
