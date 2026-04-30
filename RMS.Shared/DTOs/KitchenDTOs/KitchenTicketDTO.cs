using RMS.Domain.Enums;

namespace RMS.Shared.DTOs.KitchenDTOs
{
    public class OrderKitchenTicketDTO
    {
            public int Id { get; set; }

            public int OrderId { get; set; }

            public string Station { get; set; } = string.Empty;

            public TicketStatus Status { get; set; }

            public DateTime? StartedAt { get; set; }

            public DateTime? CompletedAt { get; set; }
        
    }
}
