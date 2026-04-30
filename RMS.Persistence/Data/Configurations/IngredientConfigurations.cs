using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations;

public class IngredientConfigurations : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(i => i.CreatedAt)
       .HasDefaultValueSql("GETDATE()");

        // ── Enum → string ─────────────────────────────────────────────────────
        builder.Property(i => i.Unit)
               .IsRequired()
               .HasMaxLength(20)
               .HasConversion<string>();

        // ── Relationships ─────────────────────────────────────────────────────
        builder.HasMany(i => i.Recipes)
               .WithOne(r => r.Ingredient)
               .HasForeignKey(r => r.IngredientId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(i => i.BranchStocks)
               .WithOne(bs => bs.Ingredient)
               .HasForeignKey(bs => bs.IngredientId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(i => !i.IsDeleted);
    }
}
