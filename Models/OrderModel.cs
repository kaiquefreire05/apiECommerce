namespace ECommerceApi.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
        public virtual ICollection<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }
}
