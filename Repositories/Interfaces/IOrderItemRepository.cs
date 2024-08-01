using ECommerceApi.Models;

namespace ECommerceApi.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<OrderItemModel> CreateOrderItem(OrderItemModel item);
        Task<List<OrderItemModel>> GetAllOrderItem();
        Task<OrderItemModel> GetOrderItemById(int id);
        Task<OrderItemModel> UpdateOrderItem(OrderItemModel item, int id);
        Task<bool> DeleteOrderItem(int id);
    }
}
