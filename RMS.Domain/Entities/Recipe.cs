using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Domain.Entities;

// Junction table: MenuItem ↔ Ingredient (Many-to-Many with payload)
public class Recipe : BaseEntity
{
    // ── FK → MenuItem ─────────────────────────────────────────────────────────
    [Required]
    public int MenuItemId { get; set; }

    [ForeignKey(nameof(MenuItemId))]
    public MenuItem? MenuItem { get; set; }

    // ── FK → Ingredient ───────────────────────────────────────────────────────
    [Required]
    public int IngredientId { get; set; }

    [ForeignKey(nameof(IngredientId))]
    public Ingredient? Ingredient { get; set; }

    // ── Payload ───────────────────────────────────────────────────────────────
    [Required]
    [Column(TypeName = "decimal(10,3)")]
    [Range(0.001, 9999)]
    public decimal QuantityRequired { get; set; }
}
