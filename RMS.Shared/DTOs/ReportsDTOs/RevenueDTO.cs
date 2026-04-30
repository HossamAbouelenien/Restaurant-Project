namespace RMS.Shared.DTOs.ReportsDTOs
{
    public class RevenueDTO
    {
        public DateTime Date { get; set; }
        public int BranchId { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
