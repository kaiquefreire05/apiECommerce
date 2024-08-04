using AutoMapper;
using ECommerceApi.DTOs;
using ECommerceApi.Models;
using ECommerceApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        // Injecting database methods and mapping
        private readonly IOrderItemRepository _rep;
        private readonly IMapper _map;
        public OrderItemController(IOrderItemRepository rep, IMapper map)
        {
            _rep = rep;
            _map = map;
        }

        // API Methods
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderItemDTO>> DeleteOrderItem(int id)
        {
            var deleted = await _rep.DeleteOrderItem(id);
            return Ok(deleted);
        }

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
