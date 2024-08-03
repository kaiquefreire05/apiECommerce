using ECommerceApi.Enums;
using System.Globalization;

namespace ECommerceApi.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public CategoriesEnum Category { get; set; }
        public List<OrderItemDTO> OrdersItemDTO { get; set; }
    }
}
