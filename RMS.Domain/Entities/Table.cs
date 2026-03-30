using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Domain.Entities;

public class Table : BaseEntity
{
    // ── FK → Branch ───────────────────────────────────────────────────────────
    [Required]
    public int BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public Branch? Branch { get; set; }

    // ── Table data ────────────────────────────────────────────────────────────
    [Required]
    [MaxLength(10)]
    public string TableNumber { get; set; } = string.Empty;

    public bool IsOccupied { get; set; } = false;

    [Range(1, 20)]
    public int Capacity { get; set; }

    // ── Navigation ───────────────────────────────────────────────────────────
    public ICollection<TableOrder> TableOrders { get; set; } = new List<TableOrder>();
}
