using ECommerceApi.Models;
using ECommerceApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        // Injecting database methods
        private readonly IOrderItemRepository _rep;
        public OrderItemController(IOrderItemRepository rep)
        {
            _rep = rep;
        }

        // API Methods
        [HttpPost]
        public async Task<ActionResult<OrderItemModel>> CreateOrderItem([FromBody] OrderItemModel orderItem)
        {
            await _rep.CreateOrderItem(orderItem);
            return CreatedAtAction(nameof(GetOrderItemById), new { id = orderItem.Id }, orderItem);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderItemModel>> DeleteOrderItem(int id)
        {
            var deleted = await _rep.DeleteOrderItem(id);
            return Ok(deleted);
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderItemModel>>> GetAllOrderItem()
        {
            return Ok(await _rep.GetAllOrderItem());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItemModel>> GetOrderItemById(int id)
        {
            var findOI = await _rep.GetOrderItemById(id);
            if (findOI == null)
            {
                return NotFound();
            }
            return Ok(findOI);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderItemModel>> UpdateOrderItem([FromBody] OrderItemModel orderItemModel, int id)
        {
            try
            {
                var updatedOI = await _rep.UpdateOrderItem(orderItemModel, id);
                return Ok(updatedOI);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
