using ECommerceApi.Database;
using ECommerceApi.Models;
using ECommerceApi.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Repositories
{
    public class OrderRepository : IOrdersRepository
    {
        // Injecting database dependency
        private readonly ECommerceDBContext _context;
        private readonly ILogger<OrderRepository> _logger;
        public OrderRepository(ECommerceDBContext context, ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Methods
        public async Task<OrderModel> CreateOrder(OrderModel order)
        {
            // Verifyng if order is null
            if (order == null)
            {
                _logger.LogError("Attempted to create a null order.");
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            }
            // Verifyng if order items is null
            if (order.OrderItems == null || !order.OrderItems.Any())
            {
                _logger.LogWarning($"Order with ID: {order.Id} has no items.");
                throw new ArgumentException("Order must contain at least one item.", nameof(order));
            }
            // Verifyng if users exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == order.UserId);
            if (!userExists)
            {
                _logger.LogError($"User with ID {order.UserId} does not exist");
                throw new ArgumentException($"User with ID {order.UserId} does not exist");
            }

            var productIds = order.OrderItems.Select(i => i.ProductId).Distinct(); // Getting all products ID
            var products = await _context.Products // Getting all products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            var productMap = products.ToDictionary(p => p.Id); // Transform in dict

            var missingProductIds = productIds.Except(productMap.Keys).ToList();
            if (missingProductIds.Any())
            {
                _logger.LogError($"One or more products referenced in OrderItems do not exist. Missing Product IDs: {string.Join(", ", missingProductIds)}");
                throw new ArgumentException("One or more products referenced in OrderItems do not exist.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var item in order.OrderItems)
                {
                    // Verify if products exists
                    if (!productMap.TryGetValue(item.ProductId, out var product))
                    {
                        _logger.LogError($"Product with ID: {item.ProductId} does not exist.");
                        throw new ArgumentException($"Product with ID: {item.ProductId} does not exist.");
                    }
                    // Verifyng product stock
                    if (product.Stock < item.Quantity)
                    {
                        _logger.LogError($"Insufficient stock for Product ID: {item.ProductId}. Available: {product.Stock}, Requested: {item.Quantity}.");
                        throw new InvalidOperationException($"Insufficient stock for Product ID: {item.ProductId}.");
                    }

                    // Update product stock
                    product.Stock -= item.Quantity;
                    _context.Products.Update(product);

                    // Set order ID in order item
                    item.OrderId = order.Id;
                    _context.OrderItems.Add(item);  // Add orderitem in context
                }

                // Add order in databse
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();  // Save changes

                await transaction.CommitAsync();  // Confirm transaction
                return order;
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx)
            {
                _logger.LogError(ex, "Database update exception while creating order.");
                await transaction.RollbackAsync();  // Rollback transaction in case error

                if (sqlEx.Number == 547)
                {
                    throw new ArgumentException("Database constraint violation.");
                }
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while creating order.");
                await transaction.RollbackAsync();  // Rollback transaction in case error
                throw new InvalidOperationException("An error occurred while creating the order.", ex);
            }
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
