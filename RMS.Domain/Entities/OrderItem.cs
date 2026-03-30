using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Domain.Entities;

public class OrderItem : BaseEntity
{
    // ── FK → Order ────────────────────────────────────────────────────────────
    [Required]
    public int OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order? Order { get; set; }

    // ── FK → MenuItem ─────────────────────────────────────────────────────────
    [Required]
    public int MenuItemId { get; set; }

    [ForeignKey(nameof(MenuItemId))]
    public MenuItem? MenuItem { get; set; }

    // ── Line data ─────────────────────────────────────────────────────────────
    [Required]
    [Range(1, 999)]
    public int Quantity { get; set; }

    // Snapshotted from MenuItem.Price at order time
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal UnitPrice { get; set; }

    [MaxLength(300)]
    public string? Notes { get; set; }
}
