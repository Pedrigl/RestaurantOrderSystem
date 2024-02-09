using Domain.Entities;
using Infrastructure.Data.Contexts;
using Infrastructure.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private new readonly RestaurantDbContext _context;

        public OrderRepository(RestaurantDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
