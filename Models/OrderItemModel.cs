namespace ECommerceApi.Models
{
    public class OrderItemModel
    {
        public required int Id { get; set; }
        public required int Quantity { get; set; }
        public required int OrderId { get; set; }
        public required int ProductId { get; set; }
        public required virtual OrderModel Order { get; set; }
        public required virtual ProductsModel Products { get; set; }
    }
}
