namespace RMS.Shared.DTOs.DeliveryDTOs
{
    public class OrderSummaryDto
    {
        public int Id { get; set; }

        public string OrderType { get; set; } = default!;

        public string Status { get; set; } = default!;

        public decimal TotalAmount { get; set; }

        public int ItemsCount { get; set; }

        public string? BranchName { get; set; }
        public string? BranchArabicName { get; set; }


        public DateTime CreatedAt { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();

        public decimal CalculatedTotal
        {
            get => Items?.Sum(i => i.Total) ?? 0;
        }
    }
}
