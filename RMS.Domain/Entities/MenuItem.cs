using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Domain.Entities;

public class MenuItem : BaseEntity
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string ArabicName { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, 999999.99)]
    public decimal Price { get; set; }

    public bool IsAvailable { get; set; } = true;

    [Range(1, 300)]
    public int PrepTimeMinutes { get; set; }


    // ── FK → Category ─────────────────────────────────────────────────────────
    [Required]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }

    // ── Navigation ───────────────────────────────────────────────────────────
    public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    
    public string? ImageUrl { get; set; }
    public string? ImagePublicId { get; set; }
}
