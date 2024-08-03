using ECommerceApi.Database;
using ECommerceApi.DTOs;
using ECommerceApi.Models;
using ECommerceApi.Repositories;
using ECommerceApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrdersRepository _rep;

        public OrderController(IOrdersRepository rep)
        {
            _rep = rep;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> GetOrders()
        {
            var orders = await _rep.GetAllOrders(); // List
            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found.");
            }

            // Mapping orders to DTO
            var ordersDto = orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                UserId = order.UserId,
                // Mapping items to DTO
                OrdersItemDTO = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    Id = oi.Id,
                    Quantity = oi.Quantity,
                    OrderId = oi.OrderId,
                    ProductId = oi.ProductId
                }).ToList()
            }).ToList();

            return Ok(ordersDto);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<OrderModel>> GetOrder(int id)
        {
            var order = await _rep.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            // Mapping order to DTO
            var orderDto = new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                UserId = order.UserId,

                // Mapping items to DTO
                OrdersItemDTO = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    Id = oi.Id,
                    Quantity = oi.Quantity,
                    OrderId = oi.OrderId,
                    ProductId = oi.ProductId
                }).ToList()
            };

            return Ok(orderDto);
        }

        [HttpPost]
        public async Task<ActionResult<OrderModel>> CreateOrder([FromBody] OrderModel order)
        {
            if (order == null)
            {
                return BadRequest("Order cannot be null.");
            }
            var createdOrder = await _rep.CreateOrder(order);
            // Mapping createdOrder to DTO
            var orderDto = new OrderDTO
            {
                Id = createdOrder.Id,
                OrderDate = createdOrder.OrderDate,
                UserId = createdOrder.UserId,

                // Mapping items to DTO
                OrdersItemDTO = createdOrder.OrderItems.Select(oi => new OrderItemDTO
                {
                    Id = oi.Id,
                    Quantity = oi.Quantity,
                    OrderId = oi.OrderId,
                    ProductId = oi.ProductId,
                }).ToList()
            };

            return CreatedAtAction(nameof(GetOrder), new { id = orderDto.Id }, orderDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderModel>> UpdateOrder(int id, [FromBody] OrderModel order)
        {
            if (order == null || order.Id != id)
            {
                return BadRequest("Order data is invalid.");
            }

            var updatedOrder = await _rep.UpdateOrder(order, id);
            if (updatedOrder == null)
            {
                return NotFound();
            }
            // Mapping updatedOrder to DTO
            var orderDto = new OrderDTO
            {
                Id = updatedOrder.Id,
                OrderDate = updatedOrder.OrderDate,
                UserId = updatedOrder.UserId,

                // Mapping itens to DTO
                OrdersItemDTO = updatedOrder.OrderItems.Select(oi => new OrderItemDTO
                { 
                    Id = oi.Id,
                    Quantity = oi.Quantity,
                    OrderId = oi.OrderId,
                    ProductId = oi.ProductId,
                }).ToList()
            };
            return Ok(orderDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _rep.DeleteOrder(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
