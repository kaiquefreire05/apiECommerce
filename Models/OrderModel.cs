using ECommerceApi.Enums;

namespace ECommerceApi.Models
{
    public class OrderModel
    {
        public required int Id { get; set; }
        public required DateTime OrderDate { get; set; }
        public required int UserId { get; set; }
        public required virtual UsersModel User { get; set; }

        public ICollection<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }
}
