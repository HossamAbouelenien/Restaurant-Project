using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations;

public class TableConfigurations : IEntityTypeConfiguration<Table>
{
    public void Configure(EntityTypeBuilder<Table> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.TableNumber)
               .IsRequired()
               .HasMaxLength(10);

        // ── Unique: no two tables in the same branch share the same number ─────
        builder.HasIndex(t => new { t.BranchId, t.TableNumber })
               .IsUnique();

        builder.Property(t => t.IsOccupied)
               .HasDefaultValue(false);

        builder.Property(t => t.Capacity)
               .IsRequired();

        builder.Property(t => t.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

        // ── FK → Branch ───────────────────────────────────────────────────────
        builder.HasOne(t => t.Branch)
               .WithMany(b => b.Tables)
               .HasForeignKey(t => t.BranchId)
               .OnDelete(DeleteBehavior.Restrict);

        // ── One Table → Many TableOrders ──────────────────────────────────────
        builder.HasMany(t => t.TableOrders)
               .WithOne(to => to.Table)
               .HasForeignKey(to => to.TableId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}
