using System.ComponentModel.DataAnnotations;

namespace RMS.Shared.DTOs.DeliveryDTOs
{
    public class UpdateDeliveryStatusDto
    {
        [Required]
        public string Status { get; set; } = default!;

        public decimal? CashCollected { get; set; }
    }
}
