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

            builder.Property(o => o.Id).ValueGeneratedOnAdd();

            builder.Property(o => o.CustomerName).IsRequired().HasMaxLength(100);

            builder.Property(o => o.OrderDate).IsRequired();

            builder.Property(o => o.OrderType).IsRequired();

            builder.OwnsOne(o => o.DeliveryAddress, da =>
            {
                da.Property(d => d.Street).IsRequired().HasMaxLength(100);
                da.Property(d => d.Number).IsRequired();
                da.Property(d => d.City).IsRequired().HasMaxLength(100);
                da.Property(d => d.State).IsRequired().HasMaxLength(100);
                da.Property(d => d.Country).IsRequired().HasMaxLength(100);
                da.Property(d => d.ZipCode).IsRequired().HasMaxLength(10);
            });

        }
    }
}
