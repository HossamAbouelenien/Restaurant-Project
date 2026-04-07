using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.ServicesAbstraction;
using RMS.Shared.DTOs.ReportsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.ReportServices
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public async Task<DashboardDTO> GetDashboardAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var orders = await _unitOfWork.GetRepository<Order>().GetAllAsync();

            if (orders == null)
                throw new Exception("Failed to retrieve orders");

            var todayOrders = orders
                .Where(o => !o.IsDeleted &&
                            o.CreatedAt >= today &&
                            o.CreatedAt < tomorrow &&
                            o.Status != OrderStatus.Cancelled)
                .ToList();

            return new DashboardDTO
            {
                OrdersToday = todayOrders.Count,
                TotalRevenueToday = todayOrders.Sum(o => o.TotalAmount),
                AvgOrderValueToday = todayOrders.Any()
                    ? todayOrders.Average(o => o.TotalAmount)
                    : 0
            };
        }

        public async Task<IEnumerable<RevenueDTO>> GetRevenueAsync(int? branchId, DateTime? from, DateTime? to)
        {
            var fromDate = from ?? DateTime.Today;
            var toDate = to ?? DateTime.Today.AddDays(1);

            var orders = await _unitOfWork.GetRepository<Order>().GetAllAsync();

            var filtered = orders.Where(o =>
                !o.IsDeleted &&
                o.Status != OrderStatus.Cancelled &&
                o.CreatedAt >= fromDate &&
                o.CreatedAt < toDate
            );

            if (branchId.HasValue)
            {
                filtered = filtered.Where(o => o.BranchId == branchId.Value);
            }

            var result = filtered
                .GroupBy(o => new { Date = o.CreatedAt.Date, o.BranchId })
                .Select(g => new RevenueDTO
                {
                    Date = g.Key.Date,
                    BranchId = g.Key.BranchId,
                    TotalRevenue = g.Sum(o => o.TotalAmount)
                })
                .OrderBy(r => r.Date)
                .ToList();

            return result;
        }
    }
}
