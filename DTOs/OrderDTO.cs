namespace ECommerceApi.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; }
        public List<OrderItemDTO> OrdersItemDTO { get; set; }
    }
}
