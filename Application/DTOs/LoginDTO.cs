using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class LoginDTO
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? JWtToken { get; set; }
        public DateTime? TokenExpiration { get; set; }
        public AccessLevel AccessLevel { get; set; }
    }
}
