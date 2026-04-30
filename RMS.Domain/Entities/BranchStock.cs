using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Domain.Entities;


public class BranchStock : BaseEntity
{
    // ── FK → Branch ───────────────────────────────────────────────────────────
    [Required]
    public int BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public Branch? Branch { get; set; }

    // ── FK → Ingredient ───────────────────────────────────────────────────────
    [Required]
    public int IngredientId { get; set; }

    [ForeignKey(nameof(IngredientId))]
    public Ingredient? Ingredient { get; set; }

    // ── Stock data ────────────────────────────────────────────────────────────
    [Column(TypeName = "decimal(10,3)")]
    [Range(0, 999999)]
    public decimal QuantityAvailable { get; set; }

    [Column(TypeName = "decimal(10,3)")]
    [Range(0, 999999)]
    public decimal LowThreshold { get; set; }

    
}
