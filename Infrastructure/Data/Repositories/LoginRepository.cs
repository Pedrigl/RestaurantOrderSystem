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
    public class LoginRepository : Repository<Login>, ILoginRepository
    {
        private new readonly RestaurantDbContext _context;

        public LoginRepository(RestaurantDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
