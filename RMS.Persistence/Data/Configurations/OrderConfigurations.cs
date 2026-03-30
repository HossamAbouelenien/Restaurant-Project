using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using RMS.Domain.Enums;

namespace RMS.Infrastructure.Configurations;

public class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        // ── Enums → string ────────────────────────────────────────────────────
        builder.Property(o => o.OrderType)
               .IsRequired()
               .HasMaxLength(20)
               .HasConversion<string>();

        builder.Property(o => o.Status)
               .IsRequired()
               .HasMaxLength(20)
               .HasConversion<string>()
               .HasDefaultValue(OrderStatus.Received);

        builder.Property(o => o.TotalAmount)
               .IsRequired()
               .HasColumnType("decimal(10,2)")
               .HasDefaultValue(0);

        builder.Property(o => o.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

        // ── FK → Branch ───────────────────────────────────────────────────────
        builder.HasOne(o => o.Branch)
               .WithMany(b => b.Orders)
               .HasForeignKey(o => o.BranchId)
               .OnDelete(DeleteBehavior.Restrict);

        // ── FK → User (customer) ──────────────────────────────────────────────
        builder.HasOne(o => o.Customer)
               .WithMany(u => u.Orders)
               .HasForeignKey(o => o.CustomerId)
               .OnDelete(DeleteBehavior.Restrict);

        // ── One Order → Many OrderItems ───────────────────────────────────────
        builder.HasMany(o => o.OrderItems)
               .WithOne(oi => oi.Order)
               .HasForeignKey(oi => oi.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        // ── One Order → Many KitchenTickets ──────────────────────────────────
        builder.HasMany(o => o.KitchenTickets)
               .WithOne(kt => kt.Order)
               .HasForeignKey(kt => kt.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        // ── One Order → One Payment (1-to-1) ─────────────────────────────────
        // FK lives on Payment side
        builder.HasOne(o => o.Payment)
               .WithOne(p => p.Order)
               .HasForeignKey<Payment>(p => p.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        // ── One Order → One Delivery (1-to-1) ────────────────────────────────
        builder.HasOne(o => o.Delivery)
               .WithOne(d => d.Order)
               .HasForeignKey<Delivery>(d => d.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        // ── One Order → One TableOrder (1-to-1) ──────────────────────────────
        builder.HasOne(o => o.TableOrder)
               .WithOne(to => to.Order)
               .HasForeignKey<TableOrder>(to => to.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(o => !o.IsDeleted);
    }
}
