using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtilities.Data
{
    public class FakeDbContext
    {
        public static RestaurantDbContext GetFakeDbContext()
        {
            var options = new DbContextOptionsBuilder<RestaurantDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new RestaurantDbContext(options);
        }
    }
}
