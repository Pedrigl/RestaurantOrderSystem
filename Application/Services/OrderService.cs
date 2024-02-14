using Application.DTOs;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Validation;
using Infrastructure.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService : IOrderService
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

        public async Task<OrderDTO> PlaceOrder(OrderDTO order)
        {
            var mappedOrder = _mapper.Map<Order>(order);
            var newOrder = await _orderRepository.AddAsync(mappedOrder);
            await _orderRepository.SaveAsync();

            return _mapper.Map<OrderDTO>(newOrder);
        }
        
        public async Task UpdateOrder(OrderDTO order)
        {
            var mappedOrder = _mapper.Map<Order>(order);
            _orderRepository.Update(mappedOrder.Id, mappedOrder);
            await _orderRepository.SaveAsync();
        }

        public async Task DeleteOrder(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            
            if(order == null)
                throw new Exception("Order not found");

            _orderRepository.Delete(order);
            await _orderRepository.SaveAsync();
        }

        public async Task<OrderDTO> GetOrderById(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<ErrorValidation> checkIfProductIsValid(OrderDTO order)
        {
            var validation = new ErrorValidation();
            validation.isValid = true;
            var product = await _productRepository.GetByIdAsync(order.ProductId);

            if (product == null)
            {
                validation.isValid = false;
                validation.validationErrors.Add($"Product with id {order.ProductId} does not exist");
            }

            else
            {
                if (product.Stock < 1)
                {
                    validation.isValid = false;
                    validation.validationErrors.Add($"Product with id {product.Id} does not have enough stock");
                }
            }

            return validation;
        }

        public IEnumerable<OrderDTO> GetOrdersByKitchenArea(KitchenArea kitchenArea)
        {
            var products = _productRepository.GetWhere(p => p.KitchenArea == kitchenArea);
            var kitchenOrders = new List<Order>();

            foreach (var product in products)
            {
                var orders = _orderRepository.GetWhere(o => o.ProductId == product.Id && o.IsDone == false);
                kitchenOrders.AddRange(orders);
            }

            return _mapper.Map<IEnumerable<OrderDTO>>(kitchenOrders);
        }
    }
}
