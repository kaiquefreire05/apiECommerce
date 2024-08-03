using ECommerceApi.Enums;
using ECommerceApi.Models;
using ECommerceApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Injecting database methods
        private readonly IUserRepository _rep;
        public UserController(IUserRepository rep)
        {
            _rep = rep;
        }

        // API Methods
        [HttpPost]
        public async Task<ActionResult<UsersModel>> CreateUser([FromBody] UsersModel user)
        {
            await _rep.CreateUser(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UsersModel>> DeleteUser(int id)
        {
            var deleted = await _rep.DeleteUser(id);
            return Ok(deleted);
        }

        [HttpGet]
        public async Task<ActionResult<List<UsersModel>>> GetAllUser()
        {
            return Ok(await _rep.GetAllUser());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsersModel>> GetUserById(int id)
        {
            var findUser = await _rep.GetUserById(id);
            if (findUser == null)
            {
                return NotFound();
            }
            return Ok(findUser);
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<UsersModel>> GetUserByNickname(string username)
        {
            var findUser = await _rep.GetUserByNickname(username);
            if (findUser == null)
            {
                return NotFound();
            }
            return Ok(findUser);
        }

        [HttpGet("role/{role}")]
        public async Task<ActionResult<List<UsersModel>>> GetUsersByRole(UserRole role)
        {
            return Ok(await _rep.GetUserByRole(role));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UsersModel>> UpdateUser([FromBody] UsersModel user, int id)
        {
            try
            {
                var updatedUser = await _rep.UpdateUser(user, id);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
