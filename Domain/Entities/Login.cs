using Domain.Enums;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Login
    {
        public int Id { get; set; }
        public string? DisplayName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public AccessLevel AccessLevel { get; set; }
    }
}
