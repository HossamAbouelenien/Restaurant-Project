using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(u => u.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

        builder.ToTable(Tb =>
        {
            Tb.HasCheckConstraint("UserValidEmailCheck", "Email LIKE '_%@_%._%'");
        });

        // ── Owned Entity: Address ─────────────────────────────────────────────
        // Address has no own table — its columns live inside the Branches table.
        builder.OwnsMany(b => b.Addresses, address =>
        {
            address.Property(a => a.BuildingNumber)
                   .HasColumnName("BuildingNumber")
                   .IsRequired();

            address.Property(a => a.Street)
                   .HasColumnName("Street")
                   .IsRequired()
                   .HasMaxLength(150);

            address.Property(a => a.City)
                   .HasColumnName("City")
                   .IsRequired()
                   .HasMaxLength(100);

            address.Property(a => a.Note)
                   .HasColumnName("Note")
                   .HasMaxLength(300);

            address.Property(a => a.SpecialMark)
                   .HasColumnName("SpecialMark")
                   .HasMaxLength(200);
        });

        // ── FK → Branch (nullable) ────────────────────────────────────────────
        builder.HasOne(u => u.Branch)
               .WithMany(b => b.Users)
               .HasForeignKey(u => u.BranchId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Restrict);

        // ── Orders placed by this user (as customer) ──────────────────────────
        builder.HasMany(u => u.Orders)
               .WithOne(o => o.User)
               .HasForeignKey(o => o.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        // ── Deliveries assigned to this user (as driver) ──────────────────────
        builder.HasMany(u => u.Deliveries)
               .WithOne(d => d.Driver)
               .HasForeignKey(d => d.DriverId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}