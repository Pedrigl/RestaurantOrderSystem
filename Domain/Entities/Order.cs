using Domain.Enums;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public required string CustomerName { get; set; }
        public int TableNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public required List<Product> Products { get; set; }
        public OrderType OrderType { get; set; }
        public DeliveryAddress? DeliveryAddress { get; set; }
    }
}
