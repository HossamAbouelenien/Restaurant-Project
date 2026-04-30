using RMS.Domain.Enums;

namespace RMS.Shared.DTOs.KitchenDTOs
{
    public class UpdateTicketStatusRequestDto
    {
        public TicketStatus Status { get; set; }
    }
}
