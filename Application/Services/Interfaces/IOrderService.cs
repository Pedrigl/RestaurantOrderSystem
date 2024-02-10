using Application.DTOs;
using Domain.Enums;
using Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllOrders();
        Task<OrderDTO> CreateOrder(OrderDTO order);
        Task<OrderDTO> UpdateOrder(OrderDTO order);
        Task DeleteOrder(int id);
        Task<OrderDTO> GetOrderById(int id);
        Task<ErrorValidation> checkIfProductIsValid(OrderDTO order);
        IEnumerable<OrderDTO> GetOrdersByKitchenArea(KitchenArea kitchenArea);
    }
}
