using RMS.Shared.DTOs.AddressDTOs;

namespace RMS.Shared.DTOs.DeliveryDTOs
{
    public class DeliveryDetailsDto
    {
        public int Id { get; set; }

        public string DeliveryStatus { get; set; } = default!;

        public DateTime CreatedAt { get; set; }

        public DateTime? DeliveredAt { get; set; }

        public decimal? CashCollected { get; set; }

        public string? DriverName { get; set; }
        public string? DriverNumber { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhoneNumber { get; set; }

        public OrderSummaryDto Order { get; set; } = default!;

        public AddressDto DeliveryAddress { get; set; } = default!;
    }
}
