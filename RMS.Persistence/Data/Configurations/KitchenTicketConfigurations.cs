using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using RMS.Domain.Enums;

namespace RMS.Infrastructure.Configurations;

public class KitchenTicketConfigurations : IEntityTypeConfiguration<KitchenTicket>
{
    public void Configure(EntityTypeBuilder<KitchenTicket> builder)
    {
        builder.HasKey(kt => kt.Id);

        builder.Property(kt => kt.Station)
               .IsRequired()
               .HasMaxLength(50);
        builder.Property(kt => kt.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

        // ── Enum → string ─────────────────────────────────────────────────────
        builder.Property(kt => kt.Status)
               .IsRequired()
               .HasMaxLength(20)
               .HasConversion<string>()
               .HasDefaultValue(TicketStatus.Pending);

        builder.Property(kt => kt.StartedAt)
               .IsRequired(false);

        builder.Property(kt => kt.CompletedAt)
               .IsRequired(false);

        // ── FK → Order ────────────────────────────────────────────────────────
        builder.HasOne(kt => kt.Order)
               .WithMany(o => o.KitchenTickets)
               .HasForeignKey(kt => kt.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(kt => !kt.IsDeleted);
    }
}
