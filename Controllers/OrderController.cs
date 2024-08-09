using AutoMapper;
using ECommerceApi.DTOs;
using ECommerceApi.Models;
using ECommerceApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers
{
    [Authorize]
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

        /// <summary>
        /// Retrieves all orders.
        /// </summary>
        /// <returns>Returns a list of all orders.</returns>
        /// <response code="200">Returns the list of orders</response>
        /// <response code="404">If no orders are found</response>
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

        /// <summary>
        /// Retrieves a specific order by ID.
        /// </summary>
        /// <param name="id">The order ID.</param>
        /// <returns>Returns the order details.</returns>
        /// <response code="200">Returns the order details</response>
        /// <response code="404">If the order is not found</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
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

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="orderDto">The order data transfer object containing order details.</param>
        /// <returns>Returns the created order details.</returns>
        /// <response code="201">Returns the newly created order</response>
        /// <response code="400">If the order is null</response>
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

            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrderDto.Id }, createdOrderDto);
        }

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        /// <param name="id">The order ID.</param>
        /// <param name="orderDto">The order data transfer object containing updated order details.</param>
        /// <returns>Returns the updated order details.</returns>
        /// <response code="200">If the order was successfully updated</response>
        /// <response code="400">If the order data is invalid</response>
        /// <response code="404">If the order is not found</response>
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

        /// <summary>
        /// Deletes an order by ID.
        /// </summary>
        /// <param name="id">The order ID.</param>
        /// <returns>Returns the status of the deletion.</returns>
        /// <response code="200">If the order was successfully deleted</response>
        /// <response code="404">If the order is not found</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var deleted = await _rep.DeleteOrder(id);
            return Ok(deleted);
        }
    }
}
