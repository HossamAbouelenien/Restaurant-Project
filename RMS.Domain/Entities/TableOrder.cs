using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Domain.Entities;

// Junction: Table ↔ Order (one sitting per record)
public class TableOrder : BaseEntity
{
    // ── FK → Table ────────────────────────────────────────────────────────────
    [Required]
    public int TableId { get; set; }

    [ForeignKey(nameof(TableId))]
    public Table? Table { get; set; }

    // ── FK → Order ────────────────────────────────────────────────────────────
    [Required]
    public int OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order? Order { get; set; }

    // ── Sitting data ──────────────────────────────────────────────────────────
    //SeatedAt => CreatedAt with fluent Api
    //public DateTime SeatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}
