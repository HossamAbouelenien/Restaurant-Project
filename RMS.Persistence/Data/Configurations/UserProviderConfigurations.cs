using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Persistence.Data.Configurations
{
    public class UserProviderConfigurations : IEntityTypeConfiguration<UserProvider>
    {
        public void Configure(EntityTypeBuilder<UserProvider> builder)
        {
            builder.HasOne(up => up.User)
                   .WithMany(u => u.Providers)
                   .HasForeignKey(up => up.UserId);

            builder.HasIndex(up => new { up.Provider, up.ProviderId })
                   .IsUnique();
        

        }
    }
}
