using ECommerceApi.Enums;
using ECommerceApi.Models;

namespace ECommerceApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserModel> CreateUser(UserModel user);
        Task<List<UserModel>> GetAllUser();
        Task<UserModel> GetUserById(int id);
        Task<List<UserModel>> GetUserByRole(UserRole role);
        Task<UserModel> GetUserByNickname(string username);
        Task<UserModel> UpdateUser(UserModel user, int id);
        Task<bool> DeleteUser(int id);
    }
}
