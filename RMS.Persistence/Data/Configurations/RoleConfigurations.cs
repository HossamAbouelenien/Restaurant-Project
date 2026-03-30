using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Configurations;

public class RoleConfigurations : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.RoleName)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasIndex(r => r.RoleName)
               .IsUnique();

        builder.Property(r => r.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}
