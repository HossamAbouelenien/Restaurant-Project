using RMS.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RMS.Domain.Entities;


public class Payment : BaseEntity
{
    // ── FK → Order (1-to-1) ───────────────────────────────────────────────────
    [Required]
    public int OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order? Order { get; set; }

    // ── Payment data ──────────────────────────────────────────────────────────
    [Required]
    [MaxLength(30)]
    public PaymentMethod PaymentMethod { get; set; }

    [Required]
    [MaxLength(20)]
    public PaymentStatus PaymentStatus { get; set; } 

    [Column(TypeName = "decimal(10,2)")]
    public decimal PaidAmount { get; set; }

    public DateTime? PaidAt { get; set; }
}
