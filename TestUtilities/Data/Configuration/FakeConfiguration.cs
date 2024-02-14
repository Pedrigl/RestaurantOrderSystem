using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtilities.Data.Configuration
{
    public class FakeConfiguration
    {
        public static IConfiguration GetFakeConfiguration()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"Jwt:Key", "Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kqw5R2Nmf4FWs03Hdx"}
            };
            IConfiguration configuration = new ConfigurationBuilder()
                                            .AddInMemoryCollection(myConfiguration)
                                            .Build();
                
            return configuration;
        }
    }
}
