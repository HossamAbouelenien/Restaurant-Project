using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations;

public class BranchStockConfigurations : IEntityTypeConfiguration<BranchStock>
{
    public void Configure(EntityTypeBuilder<BranchStock> builder)
    {
        builder.HasKey(bs => bs.Id);

        // ── Unique: one stock row per ingredient per branch ───────────────────
        builder.HasIndex(bs => new { bs.BranchId, bs.IngredientId })
               .IsUnique();

        builder.Property(bs => bs.QuantityAvailable)
               .IsRequired()
               .HasColumnType("decimal(10,3)")
               .HasDefaultValue(0);

        builder.Property(bs => bs.LowThreshold)
               .IsRequired()
               .HasColumnType("decimal(10,3)")
               .HasDefaultValue(0);

        builder.Property(bs => bs.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

        // ── UpdatedAt used instead of a separate LastUpdated column ───────────
        // (handled automatically in AppDbContext.SaveChangesAsync)
        builder.Property(X => X.UpdatedAt)
               .HasColumnName("LastUpdated");
               //.HasDefaultValueSql("GETDATE()");


        // ── FK → Branch ───────────────────────────────────────────────────────
        builder.HasOne(bs => bs.Branch)
               .WithMany(b => b.BranchStocks)
               .HasForeignKey(bs => bs.BranchId)
               .OnDelete(DeleteBehavior.Restrict);

        // ── FK → Ingredient ───────────────────────────────────────────────────
        builder.HasOne(bs => bs.Ingredient)
               .WithMany(i => i.BranchStocks)
               .HasForeignKey(bs => bs.IngredientId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(bs => !bs.IsDeleted);
    }
}
