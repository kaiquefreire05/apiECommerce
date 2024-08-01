using ECommerceApi.Database;
using ECommerceApi.Enums;
using ECommerceApi.Models;
using ECommerceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        // Injecting database dependency
        private readonly ECommerceDBContext _context;
        public UserRepository(ECommerceDBContext context)
        {
            _context = context; 
        }

        // Methods
        public async Task<UsersModel> CreateUser(UsersModel user)
        {
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await GetUserById(id);
            if (user == null)
            {
                throw new Exception($"The user with ID: {id} is not founded in database.");
            }
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UsersModel>> GetAllUser()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<UsersModel> GetUserById(int id)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UsersModel> GetUserByNickname(string username)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<List<UsersModel>> GetUserByRole(UserRole role)
        {
            return await _context.User.Where(u => u.Role == role).ToListAsync();
        }

        public async Task<UsersModel> UpdateUser(UsersModel user, int id)
        {
            var findUser = await GetUserById(id);
            if (findUser == null)
            {
                throw new Exception($"The user with ID: {id} is not founded in the database.");
            }
            _context.Entry(findUser).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
