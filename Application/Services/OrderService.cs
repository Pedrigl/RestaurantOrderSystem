using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Validation;
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
            //TODO: Add error handling
            if(order != null)
            {
                _orderRepository.Delete(order);
                await _orderRepository.SaveAsync();
            }
        }

        public async Task<OrderDTO> GetOrderById(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<ErrorValidation> checkIfAllProductsAreValid(OrderDTO order)
        {
            var validation = new ErrorValidation();

            foreach (var item in order.Products)
            {
                var product = await _productRepository.GetByIdAsync(item.Id);

                if (product == null)
                {
                    validation.validationErrors.Add($"Product with id {item.Id} does not exist");
                }

                else
                {
                    if (product.Name != item.Name)
                        validation.validationErrors.Add($"Product name for product with id {item.Id} does not match");

                    if (product.Stock < order.Products.Count(i => i == item))
                        validation.validationErrors.Add($"Product with id {item.Id} does not have enough stock");
                }
                
            }

            return validation;
        }
    }
}
