using ECommerceApi.Enums;

namespace ECommerceApi.Models
{
    public class ProductsModel
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
        public required CategoriesEnum Category { get; set; }

        public ICollection<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }
}
