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
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Update OrderItems to reference the new Order
            foreach (var item in order.OrderItems)
            {
                item.OrderId = order.Id;
                _context.OrderItems.Add(item);
            }
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return false; // NotFound
            }

            // Remove associated order items
            _context.OrderItems.RemoveRange(_context.OrderItems.Where(oi => oi.OrderId == id));
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return true; // Success
        }

        public async Task<List<OrderModel>> GetAllOrders()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        public async Task<OrderModel> GetOrderById(int id)
        {
            #pragma warning disable CS8603 // Possible null reference return.
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<OrderModel>> GetOrderByUserId(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<OrderModel> UpdateOrder(OrderModel order, int id)
        {
            if (order == null || order.Id != id)
            {
                throw new ArgumentException("Order data is invalid.");
            }

            var existingOrder = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (existingOrder == null)
            {
                return null; // NotFound
            }

            // Update existing order properties
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.UserId = order.UserId;

            // Remove old order items
            _context.OrderItems.RemoveRange(existingOrder.OrderItems);

            // Add new order items
            foreach (var item in order.OrderItems)
            {
                item.OrderId = id;
                _context.OrderItems.Add(item);
            }

            await _context.SaveChangesAsync();
            return existingOrder;
        }
    }
}
