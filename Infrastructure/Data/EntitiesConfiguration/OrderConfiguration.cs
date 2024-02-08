using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.EntitiesConfiguration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            
            builder.HasKey(o => o.Id);

            builder.Property(o => o.CustomerName).IsRequired().HasMaxLength(100);

            builder.Property(o => o.TableNumber).IsRequired(false);

            builder.Property(o => o.DeliveryAddress).IsRequired(false).HasMaxLength(100);

            builder.Property(o => o.OrderDate).IsRequired();

            builder.Property(o => o.OrderType).IsRequired();

            builder.HasMany(o => o.Products).WithOne().OnDelete(DeleteBehavior.Cascade);



        }
    }
}
