using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using RMS.Domain.Enums;

namespace RMS.Infrastructure.Configurations;

public class PaymentConfigurations : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        // ── Unique: one payment per order (enforces 1-to-1) ───────────────────
        builder.HasIndex(p => p.OrderId)
               .IsUnique();

        // ── Enums → string ────────────────────────────────────────────────────
        builder.Property(p => p.PaymentMethod)
               .IsRequired()
               .HasMaxLength(30)
               .HasConversion<string>();

        builder.Property(p => p.PaymentStatus)
               .IsRequired()
               .HasMaxLength(20)
               .HasConversion<string>()
               .HasDefaultValue(PaymentStatus.Pending);

        builder.Property(p => p.PaidAmount)
               .IsRequired()
               .HasColumnType("decimal(10,2)")
               .HasDefaultValue(0);

        builder.Property(p => p.PaidAt)
               .IsRequired(false);

        builder.Property(p => p.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

        // ── FK → Order (1-to-1) ───────────────────────────────────────────────
        builder.HasOne(p => p.Order)
               .WithOne(o => o.Payment)
               .HasForeignKey<Payment>(p => p.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
