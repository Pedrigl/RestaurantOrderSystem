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
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> CreateOrder(OrderDTO order)
        {
            var mappedOrder = _mapper.Map<Order>(order);
            var newOrder = await _orderRepository.AddAsync(mappedOrder);
            await _orderRepository.SaveAsync();

            return _mapper.Map<OrderDTO>(newOrder);
        }
        
        public async Task<OrderDTO> UpdateOrder(OrderDTO order)
        {
            var mappedOrder = _mapper.Map<Order>(order);
            var updatedOrder = _orderRepository.Update(mappedOrder);
            await _orderRepository.SaveAsync();

            return _mapper.Map<OrderDTO>(updatedOrder);
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

        public async Task<(bool isValid, List<string> validationErrors)> checkIfAllProductsAreValid(OrderDTO order)
        {
            var validationErrors = new List<string>();
            foreach (var item in order.Products)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);

                if (product == null)
                    validationErrors.Add($"Product with id {item.ProductId} does not exist");

                if(product.Name != item.ProductName)
                    validationErrors.Add($"Product name for product with id {item.ProductId} does not match");

                if (product.Stock < item.Quantity)
                    validationErrors.Add($"Product with id {item.ProductId} does not have enough stock");
            }

            if (validationErrors.Count > 0)
                return (false, validationErrors);

            return (true, validationErrors);
        }
    }
}
