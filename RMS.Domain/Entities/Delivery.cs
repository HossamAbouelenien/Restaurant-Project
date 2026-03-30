using RMS.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RMS.Domain.Entities;


public class Delivery : BaseEntity
{
    // ── FK → Order (1-to-1) ───────────────────────────────────────────────────
    [Required]
    public int OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order? Order { get; set; }

    // ── FK → User (driver) ────────────────────────────────────────────────────
    [Required]
    public int DriverId { get; set; }

    [ForeignKey(nameof(DriverId))]
    public User? Driver { get; set; }

    // ── Delivery data ─────────────────────────────────────────────────────────
    //AssignedAt => CreatedAt with fluent Api
    //public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeliveredAt { get; set; }

    [Required]
    [MaxLength(30)]
    public DeliveryStatus DeliveryStatus { get; set; } 

    [Column(TypeName = "decimal(10,2)")]
    public decimal? CashCollected { get; set; }

    [Required]
    [MaxLength(300)]
    public string DeliveryAddress { get; set; } = string.Empty;
}
