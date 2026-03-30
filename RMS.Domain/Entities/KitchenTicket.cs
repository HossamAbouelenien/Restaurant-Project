using RMS.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Domain.Entities;

// Station: Pizza | Grill | Drinks | Salads | Desserts
// Status : Pending | Preparing | Done
public class KitchenTicket : BaseEntity
{
    // ── FK → Order ────────────────────────────────────────────────────────────
    [Required]
    public int OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order? Order { get; set; }

    // ── Ticket data ───────────────────────────────────────────────────────────
    [Required]
    [MaxLength(50)]
    public string Station { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public TicketStatus Status { get; set; } 

    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
