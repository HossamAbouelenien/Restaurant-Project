using RMS.Domain.Enums;

namespace RMS.Shared.DTOs.KitchenDTOs
{
    public class KitchenTicketStatusDto
    {
        public int Id { get; set; }
        public TicketStatus Status { get; set; }

        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public bool IsOrderReady { get; set; } 
    }
}
