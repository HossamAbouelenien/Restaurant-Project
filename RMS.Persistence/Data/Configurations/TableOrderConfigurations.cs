using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations;

public class TableOrderConfigurations : IEntityTypeConfiguration<TableOrder>
{
    public void Configure(EntityTypeBuilder<TableOrder> builder)
    {
        builder.HasKey(to => to.Id);

        // ── Unique: one order sits at exactly one table ────────────────────────
        builder.HasIndex(to => to.OrderId)
               .IsUnique();

        // ── CreatedAt used as SeatedAt ------------------─
        builder.Property(p => p.CreatedAt)
               .HasColumnName("SeatedAt")
               .HasDefaultValueSql("GETDATE()");

        builder.Property(to => to.CompletedAt)
               .IsRequired(false);

        // ── FK → Table ────────────────────────────────────────────────────────
        builder.HasOne(to => to.Table)
               .WithMany(t => t.TableOrders)
               .HasForeignKey(to => to.TableId)
               .OnDelete(DeleteBehavior.Restrict);

        // ── FK → Order (1-to-1) ───────────────────────────────────────────────
        builder.HasOne(to => to.Order)
               .WithOne(o => o.TableOrder)
               .HasForeignKey<TableOrder>(to => to.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(to => !to.IsDeleted);
    }
}
