using RMS.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace RMS.Domain.Entities;

public class Ingredient : BaseEntity
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    // kg | piece | liter | gram
    [Required]
    [MaxLength(20)]
    public IngredientsUnits Unit { get; set; }

    // ── Navigation ───────────────────────────────────────────────────────────
    public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    public ICollection<BranchStock> BranchStocks { get; set; } = new List<BranchStock>();
}
