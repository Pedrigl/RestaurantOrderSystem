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

        public void Update(int id,T entity)
        {
            //I know this causes redundancy, but entity framework would break because of the way it tracks entities, is says that the entity is already being tracked by the context
            //I think this could be fixed by checking if entity exists direclty in the repository but that kind of breaks the single responsibility principle
            var dbEntity = _context.Set<T>().Find(id);
            if (dbEntity != null)
            {
                _context.Entry(dbEntity).CurrentValues.SetValues(entity);
                _context.Entry(dbEntity).State = EntityState.Modified;
            }
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

        public IEnumerable<T> GetWhere(Func<T, bool> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
