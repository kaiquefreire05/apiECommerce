using AutoMapper;
using ECommerceApi.Database;
using ECommerceApi.DTOs;
using ECommerceApi.Mapping;
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
        // Injecting database methods and mapping
        private readonly IOrdersRepository _rep;
        private readonly IMapper _map;
        public OrderController(IOrdersRepository rep, IMapper map)
        {
            _rep = rep;
            _map = map;
        }

        // Methods
        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> GetOrders()
        {
            var orders = await _rep.GetAllOrders(); // List
            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found.");
            }

            // Mapping orders to DTO
            var ordersDto = _map.Map<List<OrderDTO>>(orders);

            return Ok(ordersDto);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await _rep.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            // Mapping order to DTO
            var orderDto = _map.Map<OrderDTO>(order);
            return Ok(orderDto);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] OrderDTO orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest("Order cannot be null.");
            }

            // Mapping DTO to OrderModel
            var order = _map.Map<OrderModel>(orderDto);
            var createdOrder = await _rep.CreateOrder(order);

            // Mapping createdOrder to DTO
            var createdOrderDto = _map.Map<OrderDTO>(createdOrder);

            return CreatedAtAction(nameof(GetOrder), new { id = createdOrderDto.Id }, createdOrderDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDTO>> UpdateOrder(int id, [FromBody] OrderDTO orderDto)
        {
            if (orderDto == null || orderDto.Id != id)
            {
                return BadRequest("Order data is invalid.");
            }

            var order = _map.Map<OrderModel>(orderDto);
            var updatedOrder = await _rep.UpdateOrder(order, id);
            if (order == null)
            {
                return NotFound();
            }

            var updatedOrderDto = _map.Map<OrderDTO>(updatedOrder);
            return Ok(updatedOrderDto);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var deleted = await _rep.DeleteOrder(id);
            return Ok(deleted);
        }
    }
}
