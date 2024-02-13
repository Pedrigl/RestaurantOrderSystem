using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions;
using Microsoft.Extensions.Configuration;
namespace Infrastructure.Security
{
    public class JwtManager
    {
        private readonly IConfigurationManager _configuration;
        public JwtManager(IConfigurationManager configuration)
        {
            _configuration = configuration;
        }



        public byte[] GenerateKey()
        {
            var key = _configuration.GetSection("Jwt").GetSection("Key").Value;

            if(key == null)
                throw new Exception("Key not found in configuration file");

            return Encoding.UTF8.GetBytes(key); ;
        }
    }
}
