using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class Repository<T> where T : class
    {
        protected readonly RestaurantDbContext _context;

        public Repository(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            var newEntity = await _context.Set<T>().AddAsync(entity);
            return newEntity.Entity;
        }

        public T Update(T entity)
        {
            return _context.Set<T>().Update(entity).Entity;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
