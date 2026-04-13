using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Domain.Entities;

public class User : IdentityUser
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; } = false;

    // ── FK → Role (required) ─────────────────────────────────────────────────
    [Required]
    public string RoleId { get; set; } //Defult

    // ── FK → Branch (nullable — Admin has no branch) ─────────────────────────
    public int? BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public Branch? Branch { get; set; }

    // ── Navigation ───────────────────────────────────────────────────────────
    public ICollection<Order> Orders { get; set; } = new List<Order>();

    public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
    public ICollection<Address> Addresses{ get; set; } = new List<Address>();


}