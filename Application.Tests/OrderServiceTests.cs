using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Infrastructure.Data.Repositories.Interfaces;
using Moq;
using Domain.Enums;
using Domain.ValueObjects;
using Application.Services.Interfaces;
using FluentAssertions;
using Infrastructure.Data.Repositories;
using Application.Tests.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Contexts;
using Application.Tests.Configuration;

namespace Application.Tests
{
    [TestClass]
    public class OrderServiceTests
    {
        private IOrderService _orderService;

        [TestInitialize]
        public async Task Initialize()
        {
            RestaurantDbContext dbContext = FakeDbContext.GetFakeDbContext();
            IOrderRepository orderRepository = new OrderRepository(dbContext);
            IProductRepository productRepository = new ProductRepository(dbContext);
            _orderService = new OrderService(orderRepository, productRepository, Configuration.AutoMapper.GetMapper());

            await productRepository.AddAsync(new Product
            {
                Id = 1,
                Name = "TestProduct",
                Price = 10,
                KitchenArea = KitchenArea.Fries,
                Description = "TestDescription",
                Stock = 10
            });

            await productRepository.AddAsync(new Product
            {
                Id = 2,
                Name = "TestProductNoStock",
                Price = 10,
                KitchenArea = KitchenArea.Grill,
                Description = "TestDescription",
                Stock = 0
            });

            await orderRepository.AddAsync(new Order
            {
                Id = 1,
                CustomerName = "TestInitCostumer",
                ProductId = 1,
                OrderType = OrderType.InStore,
                DeliveryAddress = new DeliveryAddress("TestStreet", 1, "TestCity", "TestState", "TestCountry", "TestZipCode"),
                IsDone = false,
                IsPaid = false,
                IsDelivered = false
            });

            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task ShouldCreateOrder()
        {
            var newOrder = await _orderService.PlaceOrder(new OrderDTO
            {
                CustomerName = "TestCostumer",
                ProductId = 1,
                OrderType = OrderType.Delivery,
                DeliveryAddress = new DeliveryAddress("TestStreet", 1, "TestCity", "TestState", "TestCountry", "TestZipCode"),
                IsDone = false,
                IsPaid = false,
                IsDelivered = false,
                
            });
            var persistedOrder = await _orderService.GetOrderById(newOrder.Id);

            
            persistedOrder.Should().NotBeNull();
        }

        [TestMethod]
        public async Task ProductShouldBeValid()
        {
            var order = new OrderDTO
            {
                Id = 2,
                CustomerName = "TestCostumer",
                ProductId = 1,
                OrderType = OrderType.Delivery,
                DeliveryAddress = new DeliveryAddress("TestStreet", 1, "TestCity", "TestState", "TestCountry", "TestZipCode"),
                IsDone = false,
                IsPaid = false,
                IsDelivered = false
            };

            var errorValidation = await _orderService.checkIfProductIsValid(order);
            errorValidation.isValid.Should().BeTrue();
        }

        [TestMethod]
        public async Task ProductShouldBeInvalidBecauseItIsNull()
        {
            var order = new OrderDTO
            {
                Id = 2,
                CustomerName = "TestCostumer",
                ProductId = 3,
                OrderType = OrderType.Delivery,
                DeliveryAddress = new DeliveryAddress("TestStreet", 1, "TestCity", "TestState", "TestCountry", "TestZipCode"),
                IsDone = false,
                IsPaid = false,
                IsDelivered = false
            };

            var errorValidation = await _orderService.checkIfProductIsValid(order);
            errorValidation.isValid.Should().BeFalse();
        }

        [TestMethod]
        public async Task ProductShouldBeInvalidBecauseItHasNoStock()
        {
            var order = new OrderDTO
            {
                Id = 2,
                CustomerName = "TestCostumer",
                ProductId = 2,
                OrderType = OrderType.Delivery,
                DeliveryAddress = new DeliveryAddress("TestStreet", 1, "TestCity", "TestState", "TestCountry", "TestZipCode"),
                IsDone = false,
                IsPaid = false,
                IsDelivered = false
            };

            var errorValidation = await _orderService.checkIfProductIsValid(order);
            errorValidation.isValid.Should().BeFalse();
        }

        [TestMethod]
        public async Task ShouldUpdateOrder()
        {
            var order = await _orderService.GetOrderById(1);
            order.OrderDate = DateTime.Now;
            order.IsDone = true;

            var updatedOrder = await _orderService.UpdateOrder(order);
            updatedOrder.IsDone.Should().BeTrue();
        }

        [TestMethod]
        public async Task ShouldGetOrdersBasedOnKitchenArea()
        {
            var friesOrders = _orderService.GetOrdersByKitchenArea(KitchenArea.Fries);
            friesOrders.Should().HaveCount(1);

            var newOrder = await _orderService.PlaceOrder(new OrderDTO
            {
                CustomerName = "TestCostumer",
                ProductId = 2,
                OrderType = OrderType.Delivery,
                DeliveryAddress = new DeliveryAddress("TestStreet", 1, "TestCity", "TestState", "TestCountry", "TestZipCode"),
                IsDone = false,
                IsPaid = false,
                IsDelivered = false,
            });

            var grillOrders = _orderService.GetOrdersByKitchenArea(KitchenArea.Grill);
            grillOrders.Should().HaveCount(1);

            grillOrders.Should().NotBeEquivalentTo(friesOrders);
        }


    }
}