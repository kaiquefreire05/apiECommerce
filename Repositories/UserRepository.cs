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
        public async Task<UserModel> CreateUser(UserModel user)
        {
            await _context.Users.AddAsync(user);
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
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserModel>> GetAllUser()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserModel> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserModel> GetUserByNickname(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<List<UserModel>> GetUserByRole(UserRole role)
        {
            return await _context.Users.Where(u => u.Role == role).ToListAsync();
        }

        public async Task<UserModel> UpdateUser(UserModel user, int id)
        {
            var findUser = await GetUserById(id);
            if (findUser == null)
            {
                throw new Exception($"The user with ID: {id} is not founded in the database.");
            }

            findUser.UserName = user.UserName;
            findUser.UserEmail = user.UserEmail;
            findUser.Password = user.Password;
            findUser.Role = user.Role;

            _context.Users.Update(findUser);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserModel> GetUserByUsernameAndPassword(string username, string password)
        {
            return await _context.Users.SingleOrDefaultAsync(user => user.UserName == username && user.Password == password);
        }
    }
}
