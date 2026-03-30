using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations;

public class RecipeConfigurations : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.HasKey(r => r.Id);

        // ── Unique: one ingredient appears once per menu item ─────────────────
        builder.HasIndex(r => new { r.MenuItemId, r.IngredientId })
               .IsUnique();

        builder.Property(r => r.QuantityRequired)
               .IsRequired()
               .HasColumnType("decimal(10,3)");

        builder.Property(r => r.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

        // ── FK → MenuItem ─────────────────────────────────────────────────────
        builder.HasOne(r => r.MenuItem)
               .WithMany(m => m.Recipes)
               .HasForeignKey(r => r.MenuItemId)
               .OnDelete(DeleteBehavior.Cascade);

        // ── FK → Ingredient ───────────────────────────────────────────────────
        builder.HasOne(r => r.Ingredient)
               .WithMany(i => i.Recipes)
               .HasForeignKey(r => r.IngredientId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}
