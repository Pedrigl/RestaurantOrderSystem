﻿using Application.DTOs;
using Application.Services;
using Application.Services.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrders();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderById(id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetByKitchenArea")]
        public IActionResult GetOrderByKitchenArea(KitchenArea kitchenArea)
        {
            try
            {
                var orders = _orderService.GetOrdersByKitchenArea(kitchenArea);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Place")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderDTO order)
        {
            try
            {
                var areOrderProductsValid = await _orderService.checkIfProductIsValid(order);
                if (!areOrderProductsValid.isValid)
                    return BadRequest(areOrderProductsValid);

                var newOrder = await _orderService.PlaceOrder(order);
                return Ok(newOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderDTO order)
        {
            try
            {
                var areOrderProductsValid = await _orderService.checkIfProductIsValid(order);
                if (!areOrderProductsValid.isValid)
                    return BadRequest(areOrderProductsValid);

                await _orderService.UpdateOrder(order);

                var newOrder = await _orderService.GetOrderById(order.Id);

                return Ok(newOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteOrder(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
