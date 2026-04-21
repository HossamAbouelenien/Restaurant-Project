using System.ComponentModel.DataAnnotations;

namespace RMS.Domain.Entities;

public class Branch : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = default!;
    

    public Address Address { get; set; } = default!;

    [Required]
    [MaxLength(15)]
    public string Phone { get; set; } = default!;

    public bool IsActive { get; set; } = true;

    // ── Navigation ───────────────────────────────────────────────────────────
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Table> Tables { get; set; } = new List<Table>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<BranchStock> BranchStocks { get; set; } = new List<BranchStock>();
}
