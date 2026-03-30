using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(150);

        builder.HasIndex(u => u.Email)
               .IsUnique();

        builder.Property(u => u.PasswordHash)
               .IsRequired()
               .HasMaxLength(256);

        builder.Property(u => u.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

        builder.ToTable(Tb =>
        {
            Tb.HasCheckConstraint("UserValidEmailCheck", "Email LIKE '_%@_%._%'");
        });

        // ── FK → Role ─────────────────────────────────────────────────────────
        builder.HasOne(u => u.Role)
               .WithMany(r => r.Users)
               .HasForeignKey(u => u.RoleId)
               .OnDelete(DeleteBehavior.Restrict);

        // ── FK → Branch (nullable) ────────────────────────────────────────────
        builder.HasOne(u => u.Branch)
               .WithMany(b => b.Users)
               .HasForeignKey(u => u.BranchId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Restrict);

        // ── Orders placed by this user (as customer) ──────────────────────────
        builder.HasMany(u => u.Orders)
               .WithOne(o => o.Customer)
               .HasForeignKey(o => o.CustomerId)
               .OnDelete(DeleteBehavior.Restrict);

        // ── Deliveries assigned to this user (as driver) ──────────────────────
        builder.HasMany(u => u.Deliveries)
               .WithOne(d => d.Driver)
               .HasForeignKey(d => d.DriverId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}
