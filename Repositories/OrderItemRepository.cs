using ECommerceApi.Database;
using ECommerceApi.Models;
using ECommerceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        // Injecting database dependency
        private readonly ECommerceDBContext _context;
        public OrderItemRepository(ECommerceDBContext context)
        {
            _context = context;
        }

        // Methods
        public async Task<OrderItemModel> CreateOrderItem(OrderItemModel item)
        {
            await _context.OrderItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteOrderItem(int id)
        {
            var findOI = await GetOrderItemById(id);
            if (findOI == null)
            {
                throw new Exception($"The Order Item with ID: {id} is not founded in the database.");
            }
            _context.OrderItems.Remove(findOI);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<OrderItemModel>> GetAllOrderItem()
        {
            return await _context.OrderItems
                .Include(oi => oi.Order).Include(oi => oi.Products).ToListAsync();
        }

        public async Task<OrderItemModel> GetOrderItemById(int id)
        {
            #pragma warning disable CS8603 // Possible null reference return.
            return await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Products)
                .FirstOrDefaultAsync(oi => oi.Id == id);
        }

        public async Task<OrderItemModel> UpdateOrderItem(OrderItemModel item, int id)
        {
            var itemFind = await GetOrderItemById(id);
            if (itemFind == null)
            {
                throw new Exception($"The Order Item with ID: {id} is not founded in the database");
            }
            _context.Entry(itemFind).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}
