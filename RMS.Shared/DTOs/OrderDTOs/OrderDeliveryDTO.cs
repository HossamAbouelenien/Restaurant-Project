namespace RMS.Shared.DTOs.OrderDTOs
{
    public class OrderDeliveryDTO
    {
        public string DriverId { get; set; } = default!;
        public string? DriverName { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string DeliveryStatus { get; set; }=default!;
        public decimal? CashCollected { get; set; }
        public OrderAddressDTO DeliveryAddress { get; set; } =default!;
    }
}
