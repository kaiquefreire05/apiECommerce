using AutoMapper;
using ECommerceApi.DTOs;
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
        // Injecting database methods and mapping
        private readonly IUserRepository _rep;
        private readonly IMapper _map;
        public UserController(IUserRepository rep, IMapper map)
        {
            _rep = rep;
            _map = map;
        }

        // API Methods
        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserDTO userDto)
        {
            var createdUser = await _rep.CreateUser(_map.Map<UserModel>(userDto));
            var createdUserDto = _map.Map<UserDTO>(createdUser);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUserDto.Id }, createdUserDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserModel>> DeleteUser(int id)
        {
            var deleted = await _rep.DeleteUser(id);
            return Ok(deleted);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetAllUser()
        {
            var users = await _rep.GetAllUser();
            return Ok(_map.Map<List<UserDTO>>(users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            var findUser = await _rep.GetUserById(id);
            if (findUser == null)
            {
                return NotFound();
            }
            return Ok(_map.Map<UserDTO>(findUser));
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<UserModel>> GetUserByNickname(string username)
        {
            var findUser = await _rep.GetUserByNickname(username);
            if (findUser == null)
            {
                return NotFound();
            }
            return Ok(_map.Map<UserDTO>(findUser));
        }

        [HttpGet("role/{role}")]
        public async Task<ActionResult<List<UserModel>>> GetUsersByRole(UserRole role)
        {
            var users = await _rep.GetUserByRole(role);
            return Ok(_map.Map<List<UserDTO>>(users));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserModel>> UpdateUser([FromBody] UserModel user, int id)
        {
            try
            {
                var updatedUser = await _rep.UpdateUser(user, id);
                return Ok(_map.Map<UserDTO>(updatedUser));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
