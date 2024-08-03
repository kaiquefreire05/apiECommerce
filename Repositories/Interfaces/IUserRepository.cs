using ECommerceApi.Enums;
using ECommerceApi.Models;

namespace ECommerceApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UsersModel> CreateUser(UsersModel user);
        Task<List<UsersModel>> GetAllUser();
        Task<UsersModel> GetUserById(int id);
        Task<List<UsersModel>> GetUserByRole(UserRole role);
        Task<UsersModel> GetUserByNickname(string username);
        Task<UsersModel> UpdateUser(UsersModel user, int id);
        Task<bool> DeleteUser(int id);
    }
}
