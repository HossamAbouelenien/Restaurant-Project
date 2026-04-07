using RMS.Shared.DTOs.ReportsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction
{
    public interface IReportService
    {
        Task<DashboardDTO> GetDashboardAsync();
        
        Task<IEnumerable<RevenueDTO>> GetRevenueAsync(int? branchId, DateTime? from, DateTime? to);
        Task<OrdersByTypeDTO> GetOrdersByTypeAsync();
        Task<IEnumerable<TopItemsDto>> GetTopItemsAsync(int top);
    }
}
