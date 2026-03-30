using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using RMS.Domain.Enums;

namespace RMS.Infrastructure.Configurations;

public class DeliveryConfigurations : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.HasKey(d => d.Id);

        // ── Unique: one delivery per order (enforces 1-to-1) ──────────────────
        builder.HasIndex(d => d.OrderId)
               .IsUnique();

        // ── Enum → string ─────────────────────────────────────────────────────
        builder.Property(d => d.DeliveryStatus)
               .IsRequired()
               .HasMaxLength(30)
               .HasConversion<string>()
               .HasDefaultValue(DeliveryStatus.Assigned);

        // ── CreatedAt used as AssignedAt (set automatically in SaveChangesAsync)
        builder.Property(X => X.CreatedAt)
               .HasColumnName("AssignedAt")
               .HasDefaultValueSql("GETDATE()");

        builder.Property(d => d.DeliveredAt)
               .IsRequired(false);

        builder.Property(d => d.CashCollected)
               .HasColumnType("decimal(10,2)")
               .IsRequired(false);

        builder.Property(d => d.DeliveryAddress)
               .IsRequired()
               .HasMaxLength(300);

        // ── FK → Order (1-to-1) ───────────────────────────────────────────────
        builder.HasOne(d => d.Order)
               .WithOne(o => o.Delivery)
               .HasForeignKey<Delivery>(d => d.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        // ── FK → User (driver) ────────────────────────────────────────────────
        // Restrict to avoid multiple cascade paths from User
        builder.HasOne(d => d.Driver)
               .WithMany(u => u.Deliveries)
               .HasForeignKey(d => d.DriverId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(d => !d.IsDeleted);
    }
}
