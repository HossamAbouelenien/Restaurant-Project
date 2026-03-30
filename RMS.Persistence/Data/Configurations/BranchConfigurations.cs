using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Persistence.Data.Configurations;

public class BranchConfigurations : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(b => b.Phone)
               .IsRequired()
               .HasMaxLength(15);

        builder.Property(b => b.IsActive)
               .HasDefaultValue(true);

        builder.ToTable(Tb =>
        {
            Tb.HasCheckConstraint("BranchValidPhoneCheck", "Phone LIKE '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");
        });

        builder.HasIndex(b => b.Phone).IsUnique();

        builder.Property(b => b.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

        // ── Owned Entity: Address ─────────────────────────────────────────────
        // Address has no own table — its columns live inside the Branches table.
        builder.OwnsOne(b => b.Address, address =>
        {
            address.Property(a => a.BuildingNumber)
                   .HasColumnName("BuildingNumber")
                   .IsRequired();

            address.Property(a => a.Street)
                   .HasColumnName("Street")
                   .IsRequired()
                   .HasMaxLength(150);

            address.Property(a => a.City)
                   .HasColumnName("City")
                   .IsRequired()
                   .HasMaxLength(100);

            address.Property(a => a.Note)
                   .HasColumnName("Note")
                   .HasMaxLength(300);

            address.Property(a => a.SpecialMark)
                   .HasColumnName("SpecialMark")
                   .HasMaxLength(200);
        });

        // ── Relationships ─────────────────────────────────────────────────────
        builder.HasMany(b => b.Users)
               .WithOne(u => u.Branch)
               .HasForeignKey(u => u.BranchId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.Tables)
               .WithOne(t => t.Branch)
               .HasForeignKey(t => t.BranchId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.Orders)
               .WithOne(o => o.Branch)
               .HasForeignKey(o => o.BranchId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.BranchStocks)
               .WithOne(bs => bs.Branch)
               .HasForeignKey(bs => bs.BranchId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(b => !b.IsDeleted);
    }
}
