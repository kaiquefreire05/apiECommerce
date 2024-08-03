using ECommerceApi.Models;

namespace ECommerceApi.Repositories.Interfaces
{
    public interface IOrdersRepository
    {
        Task<OrderModel> CreateOrder(OrderModel order);
        Task<List<OrderModel>> GetAllOrders();
        Task<OrderModel> GetOrderById(int id);
        Task<List<OrderModel>> GetOrderByUserId(int userId);
        Task<OrderModel> UpdateOrder(OrderModel order, int id);
        Task<bool> DeleteOrder(int id);
    }
}
