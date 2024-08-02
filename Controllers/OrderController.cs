using ECommerceApi.Models;
using ECommerceApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        // Injecting database methods
        private readonly OrderRepository _rep;
        public OrderController(OrderRepository rep)
        {
            _rep = rep;
        }

        // API methods
        [HttpPost]
        public async Task<ActionResult<OrderModel>> CreateOrder([FromBody] OrderModel order)
        {
            await _rep.CreateOrder(order);
            return CreatedAtAction(nameof(GetOrderById), new {id = order.Id}, order);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderModel>> DeleteOrder(int id)
        {
            var result = await _rep.DeleteOrder(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderModel>>> GetAllOrders()
        {
            return Ok(await _rep.GetAllOrder());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderModel>> GetOrderById(int id)
        {
            var findOrder = await _rep.GetOrderById(id);
            return Ok(findOrder);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<OrderModel>>> GetOrderByUserId(int userId)
        {
            var findOrder = await _rep.GetOrderByUserId(userId);
            return Ok(findOrder);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderModel>> UpdateOrder([FromBody] OrderModel order, int id)
        {
            try
            {
                var updatedOrder = await _rep.UpdateOrder(order, id);
                return Ok(updatedOrder);
            } 
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
