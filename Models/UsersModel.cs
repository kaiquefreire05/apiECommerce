using ECommerceApi.Enums;

namespace ECommerceApi.Models
{
    public class UsersModel
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public string? UserEmail { get; set; }
        public required string Password { get; set; }
        public required UserRole Role { get; set; }
        // Orders collection
        public ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();
    }
}
