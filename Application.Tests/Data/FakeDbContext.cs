using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
namespace Application.Tests.Data
{
    public static class FakeDbContext
    {
        public static RestaurantDbContext GetFakeDbContext()
        {
            var options = new DbContextOptionsBuilder<RestaurantDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new RestaurantDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
