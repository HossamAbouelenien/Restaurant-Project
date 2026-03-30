using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations;

public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Quantity)
               .IsRequired();

        // Snapshotted price — never updated after order is placed
        builder.Property(oi => oi.UnitPrice)
               .IsRequired()
               .HasColumnType("decimal(10,2)");

        builder.Property(oi => oi.Notes)
               .HasMaxLength(500);

        builder.Property(oi => oi.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

        // ── FK → Order ────────────────────────────────────────────────────────
        builder.HasOne(oi => oi.Order)
               .WithMany(o => o.OrderItems)
               .HasForeignKey(oi => oi.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        // ── FK → MenuItem ─────────────────────────────────────────────────────
        builder.HasOne(oi => oi.MenuItem)
               .WithMany(m => m.OrderItems)
               .HasForeignKey(oi => oi.MenuItemId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(oi => !oi.IsDeleted);
    }
}
