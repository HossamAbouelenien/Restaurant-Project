using System.ComponentModel.DataAnnotations;

namespace RMS.Domain.Entities;

public class Category : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    // ── Navigation ───────────────────────────────────────────────────────────
    public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
