using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class DeliveryAddress
    {
        public string Street { get; }
        public string Number { get; }
        public string City { get; }
        public string State { get; }
        public string Country { get; }
        public string ZipCode { get; }

        public DeliveryAddress(string street, string number, string city, string state, string country, string zipCode)
        {
            Street = street;
            Number = number;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;
        }
    }
}
