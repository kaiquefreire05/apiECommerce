using ECommerceApi.Enums;

namespace ECommerceApi.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}

