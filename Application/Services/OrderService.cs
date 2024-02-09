using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task CreateOrder(OrderDTO order)
        {
            var newOrder = _mapper.Map<Order>(order);
            await _orderRepository.AddAsync(newOrder);
            await _orderRepository.SaveAsync();
        }
        
        public async Task UpdateOrder(OrderDTO order)
        {
            var updatedOrder = _mapper.Map<Order>(order);
            _orderRepository.Update(updatedOrder);
            await _orderRepository.SaveAsync();
        }

        public async Task DeleteOrder(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            _orderRepository.Delete(order);
            await _orderRepository.SaveAsync();
        }

        public async Task<OrderDTO> GetOrderById(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<OrderDTO>(order);
        }
    }
}
