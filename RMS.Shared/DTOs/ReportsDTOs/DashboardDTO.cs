namespace RMS.Shared.DTOs.ReportsDTOs
{
    public class DashboardDTO
    {
        public decimal AvgOrderValueToday { get; set; }

        public int OrdersToday { get; set; }

        public decimal TotalRevenueToday { get; set; }

        public int LowStockIngredientsCount { get; set; }

    }
}
