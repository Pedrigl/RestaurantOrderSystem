using Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        IEnumerable<Order> GetWhere(Func<Order, bool> predicate);
        Task<Order> GetByIdAsync(int id);
        Task<Order> AddAsync(Order order);
        void Update(int orderId, Order order);
        void Delete(Order order);
        Task<bool> SaveAsync();
    }
}
