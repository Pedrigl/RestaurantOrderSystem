using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Infrastructure.Data.Repositories.Interfaces;
using Moq;
using Domain.Enums;
using Domain.ValueObjects;
namespace Application.Tests
{
    [TestClass]
    public class OrderServiceTests
    {
        private Mock<OrderService> _orderService;
        [TestInitialize]
        public void Initialize()
        {
            _orderService = new Mock<OrderService>();
            _orderService.Setup(x => x.GetAllOrders()).ReturnsAsync(new List<OrderDTO>
            {

            });
        }

        [TestMethod]
        public void shouldCreateOrder()
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

            _orderService.Setup(x => x.CreateOrder(order)).ReturnsAsync(order);
        }
    }
}