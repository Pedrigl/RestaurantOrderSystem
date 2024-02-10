using Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        IEnumerable<Product> GetWhere(Func<Product, bool> predicate);
        Task<Product> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Product Update(Product product);
        void Delete(Product product);
        Task<bool> SaveAsync();
    }
}
