using RMS.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Domain.Entities;

// OrderType  : DineIn | Pickup | Delivery
// OrderStatus: Received | Preparing | Ready | Delivered | Cancelled
public class Order : BaseEntity
{
    // ── FK → Branch ───────────────────────────────────────────────────────────
    [Required]
    public int BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public Branch? Branch { get; set; }

    // ── FK → User (customer) ─────────────────────────────────────────────────
    [Required]
    public string CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public User? Customer { get; set; }

    // ── Order type & status ───────────────────────────────────────────────────
    [Required]
    [MaxLength(20)]
    public OrderType OrderType { get; set; }

    [Required]
    [MaxLength(20)]
    public OrderStatus Status { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    // ── Navigation ───────────────────────────────────────────────────────────
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public ICollection<KitchenTicket> KitchenTickets { get; set; } = new List<KitchenTicket>();
    public Payment? Payment { get; set; }       // 1-to-1, FK lives on Payment
    public Delivery? Delivery { get; set; }     // 1-to-1, FK lives on Delivery
    public TableOrder? TableOrder { get; set; } // 1-to-1, FK lives on TableOrder
}