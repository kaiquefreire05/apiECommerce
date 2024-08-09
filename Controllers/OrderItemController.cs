using AutoMapper;
using ECommerceApi.DTOs;
using ECommerceApi.Models;
using ECommerceApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemRepository _rep;
        private readonly IMapper _map;

        public OrderItemController(IOrderItemRepository rep, IMapper map)
        {
            _rep = rep;
            _map = map;
        }

        /// <summary>
        /// Creates a new order item.
        /// </summary>
        /// <param name="orderItemDto">The order item data transfer object containing order item details.</param>
        /// <returns>Returns the created order item details.</returns>
        /// <response code="201">Returns the newly created order item</response>
        /// <response code="400">If the order item is null</response>
        [HttpPost]
        public async Task<ActionResult<OrderItemDTO>> CreateOrderItem([FromBody] OrderItemDTO orderItemDto)
        {
            if (orderItemDto == null)
            {
                return BadRequest("Order cannot be null.");
            }

            var orderItem = _map.Map<OrderItemModel>(orderItemDto);
            var createdOrderItem = await _rep.CreateOrderItem(orderItem);

            var createdOrderItemDto = _map.Map<OrderItemDTO>(createdOrderItem);
            return CreatedAtAction(nameof(GetOrderItemById), new { id = createdOrderItemDto.Id }, createdOrderItemDto);
        }

        /// <summary>
        /// Deletes an order item by ID.
        /// </summary>
        /// <param name="id">The order item ID.</param>
        /// <returns>Returns the status of the deletion.</returns>
        /// <response code="200">If the order item was successfully deleted</response>
        /// <response code="404">If the order item was not found</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderItemDTO>> DeleteOrderItem(int id)
        {
            var deleted = await _rep.DeleteOrderItem(id);
            return Ok(deleted);
        }

        /// <summary>
        /// Retrieves all order items.
        /// </summary>
        /// <returns>Returns a list of all order items.</returns>
        /// <response code="200">Returns the list of order items</response>
        /// <response code="404">If no order items are found</response>
        [HttpGet]
        public async Task<ActionResult<List<OrderItemDTO>>> GetAllOrderItem()
        {
            var ordersItem = await _rep.GetAllOrderItem();
            if (ordersItem == null || !ordersItem.Any())
            {
                return NotFound("No Orders Items found.");
            }

            var ordersItemDto = _map.Map<List<OrderItemDTO>>(ordersItem);
            return Ok(ordersItemDto);
        }

        /// <summary>
        /// Retrieves an order item by ID.
        /// </summary>
        /// <param name="id">The order item ID.</param>
        /// <returns>Returns the order item details.</returns>
        /// <response code="200">Returns the order item details</response>
        /// <response code="404">If the order item was not found</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItemDTO>> GetOrderItemById(int id)
        {
            var findOrderItem = await _rep.GetOrderItemById(id);
            if (findOrderItem == null)
            {
                return NotFound();
            }

            var findOrderItemDto = _map.Map<OrderItemDTO>(findOrderItem);
            return Ok(findOrderItemDto);
        }

        /// <summary>
        /// Updates an order item's information.
        /// </summary>
        /// <param name="orderItemDto">The order item data transfer object with updated information.</param>
        /// <param name="id">The order item ID.</param>
        /// <returns>Returns the updated order item details.</returns>
        /// <response code="200">If the order item was successfully updated</response>
        /// <response code="404">If the order item was not found</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderItemDTO>> UpdateOrderItem([FromBody] OrderItemDTO orderItemDto, int id)
        {
            try
            {
                var orderItem = _map.Map<OrderItemModel>(orderItemDto);
                var updatedOrderItem = await _rep.UpdateOrderItem(orderItem, id);
                var updatedOrderItemDto = _map.Map<OrderItemDTO>(updatedOrderItem);
                return Ok(updatedOrderItemDto);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
