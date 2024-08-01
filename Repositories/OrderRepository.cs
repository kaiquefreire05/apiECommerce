using ECommerceApi.Database;
using ECommerceApi.Models;
using ECommerceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Repositories
{
    public class OrderRepository : IOrdersRepository
    {
        // Injecting database dependency
        private readonly ECommerceDBContext _context;
        public OrderRepository(ECommerceDBContext context)
        {
            _context = context;
        }

        // Methods
        public async Task<OrderModel> CreateOrder(OrderModel order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var findOrder = await GetOrderById(id);
            if (findOrder == null)
            {
                throw new Exception($"The Order with ID: {id} is not founded in the database.");
            }
            _context.Orders.Remove(findOrder);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<OrderModel>> GetAllOrder()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Products)
                .ToListAsync();
        }

        public async Task<OrderModel> GetOrderById(int id)
        {
            #pragma warning disable CS8603 // Possible null reference return.
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Products)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<OrderModel>> GetOrderByUserId(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Products)
                .Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<OrderModel> UpdateOrder(OrderModel order, int id)
        {
            var findOrder = await GetOrderById(id);
            if (findOrder == null) 
            { 
                throw new Exception($"The Order with ID: {id} is not founded in the database.");
            }
            _context.Entry(findOrder).CurrentValues.SetValues(order);

            // Removing all existing items
            _context.OrderItems.RemoveRange(order.OrderItems);
            foreach (var item in order.OrderItems)
            {
                _context.OrderItems.Add(item);
            }
            await _context.SaveChangesAsync();
            return await GetOrderById(id);

        }
    }
}
