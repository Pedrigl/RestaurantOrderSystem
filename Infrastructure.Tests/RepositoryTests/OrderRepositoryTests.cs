using Infrastructure.Data.Repositories;
using Infrastructure.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtilities.Data;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
namespace Infrastructure.Tests.RepositoryTests
{
    [TestClass]
    public class OrderRepositoryTests
    {
        private IOrderRepository _orderRepository;

        [TestInitialize]
        public async Task Initialize()
        {
            _orderRepository = new OrderRepository(FakeDbContext.GetFakeDbContext());

            await _orderRepository.AddAsync(new Order
            {
                Id = 1,
                CustomerName = "Test Customer",
                OrderDate = DateTime.Now,
                DeliveryAddress = new DeliveryAddress("TestStreet", 1, "TestCity", "TestState", "TestCountry", "TestZipCode"),
                IsDone = false,
                IsDelivered = false,
                IsPaid = false,
                ProductId = 1
            });

            await _orderRepository.AddAsync(new Order
            {
                Id = 2,
                CustomerName = "Test Customer 2",
                OrderDate = DateTime.Now,
                DeliveryAddress = new DeliveryAddress("TestStreet", 1, "TestCity", "TestState", "TestCountry", "TestZipCode"),
                IsDone = false,
                IsDelivered = false,
                IsPaid = false,
                ProductId = 2
            });

            await _orderRepository.SaveAsync();
        }

        [TestMethod]
        public async Task GetOrders()
        {
            var orders = await _orderRepository.GetAllAsync();
            orders.Should().HaveCount(2);
        }

        [TestMethod]
        public async Task GetOrderById()
        {
            var order = await _orderRepository.GetByIdAsync(1);
            order.Should().NotBeNull();
        }

        [TestMethod]
        public async Task ShouldCreateOrder()
        {
            var order = new Order
            {
                Id = 3,
                CustomerName = "Test Customer 3",
                OrderDate = DateTime.Now,
                DeliveryAddress = new DeliveryAddress("TestStreet", 1, "TestCity", "TestState", "TestCountry", "TestZipCode"),
                IsDone = false,
                IsDelivered = false,
                IsPaid = false,
                ProductId = 3
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveAsync();

            var orders = await _orderRepository.GetAllAsync();
            orders.Should().HaveCount(3);
        }

        [TestMethod]
        public async Task ShouldUpdateOrder()
        {
            var order = await _orderRepository.GetByIdAsync(1);
            order.CustomerName = "Updated Customer";
            _orderRepository.Update(order.Id, order);
            await _orderRepository.SaveAsync();

            var updatedOrder = await _orderRepository.GetByIdAsync(1);
            updatedOrder.CustomerName.Should().Be("Updated Customer");
        }

        [TestMethod]
        public async Task ShouldDeleteOrder()
        {
            var order = await _orderRepository.GetByIdAsync(1);
            _orderRepository.Delete(order);
            await _orderRepository.SaveAsync();

            var orders = await _orderRepository.GetAllAsync();
            orders.Should().HaveCount(1);
        }
    }
}
