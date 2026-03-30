using System.ComponentModel.DataAnnotations;

namespace RMS.Domain.Entities;

public class Role : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string RoleName { get; set; } = string.Empty;
    // Admin | Waiter | Chef | Cashier | Driver | Customer

    // ── Navigation ───────────────────────────────────────────────────────────
    public ICollection<User> Users { get; set; } = new List<User>();
}
