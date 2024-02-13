using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Data.EntitiesConfiguration
{
    public class LoginConfiguration : IEntityTypeConfiguration<Login>
    {
        public void Configure(EntityTypeBuilder<Login> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Id).ValueGeneratedOnAdd();

            builder.Property(l => l.DisplayName).IsRequired().HasMaxLength(100);

            builder.Property(l => l.Username).IsRequired().HasMaxLength(100);

            builder.Property(l => l.Password).IsRequired().HasMaxLength(255);
            
            builder.Property(l => l.JWtToken).HasMaxLength(255);

            builder.Property(l => l.AccessLevel).IsRequired().HasConversion<int>();
        }
    }
}
