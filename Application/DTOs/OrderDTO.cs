﻿using Domain.Enums;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
namespace Application.DTOs
{
    public class OrderDTO
    {
        [Key]
        [SwaggerSchema(ReadOnly =true)]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public required string CustomerName { get; set; }
        public required List <ProductDTO> Products { get; set; }
        public OrderType OrderType { get; set; }
        public DeliveryAddress? DeliveryAddress { get; set; }
    }
}
