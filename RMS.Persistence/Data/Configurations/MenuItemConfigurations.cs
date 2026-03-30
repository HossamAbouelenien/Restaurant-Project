using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations;

public class MenuItemConfigurations : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(m => m.Price)
               .IsRequired()
               .HasColumnType("decimal(10,2)");

        builder.Property(m => m.IsAvailable)
               .HasDefaultValue(true);

        builder.Property(m => m.PrepTimeMinutes)
               .IsRequired();

        builder.Property(m => m.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

        // ── FK → Category ─────────────────────────────────────────────────────
        builder.HasOne(m => m.Category)
               .WithMany(c => c.MenuItems)
               .HasForeignKey(m => m.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        // ── Relationships ─────────────────────────────────────────────────────
        builder.HasMany(m => m.Recipes)
               .WithOne(r => r.MenuItem)
               .HasForeignKey(r => r.MenuItemId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.OrderItems)
               .WithOne(oi => oi.MenuItem)
               .HasForeignKey(oi => oi.MenuItemId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}
