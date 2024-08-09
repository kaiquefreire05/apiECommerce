using AutoMapper;
using ECommerceApi.DTOs;
using ECommerceApi.Enums;
using ECommerceApi.Models;
using ECommerceApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers
{
    [Authorize]
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

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userDto">The user data transfer object containing user details.</param>
        /// <returns>Returns the created user details.</returns>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="400">If the user is null</response>
        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserDTO userDto)
        {
            var createdUser = await _rep.CreateUser(_map.Map<UserModel>(userDto));
            var createdUserDto = _map.Map<UserDTO>(createdUser);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUserDto.Id }, createdUserDto);
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>Returns the status of the deletion.</returns>
        /// <response code="200">If the user was successfully deleted</response>
        /// <response code="500">If the user was not found</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserModel>> DeleteUser(int id)
        {
            var deleted = await _rep.DeleteUser(id);
            return Ok(deleted);
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>Returns a list of all users.</returns>
        /// <response code="200">Returns the list of users</response>
        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetAllUser()
        {
            var users = await _rep.GetAllUser();
            return Ok(_map.Map<List<UserDTO>>(users));
        }

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>Returns the user details.</returns>
        /// <response code="200">Returns the user details</response>
        /// <response code="500">If the user was not found</response>
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

        /// <summary>
        /// Retrieves a user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Returns the user details.</returns>
        /// <response code="200">Returns the user details</response>
        /// <response code="500">If the user was not found</response>
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

        /// <summary>
        /// Retrieves users by role.
        /// </summary>
        /// <param name="role">The user role.</param>
        /// <returns>Returns a list of users with the specified role.</returns>
        /// <response code="200">Returns the list of users</response>
        [HttpGet("role/{role}")]
        public async Task<ActionResult<List<UserModel>>> GetUsersByRole(UserRole role)
        {
            var users = await _rep.GetUserByRole(role);
            return Ok(_map.Map<List<UserDTO>>(users));
        }

        /// <summary>
        /// Updates a user's information.
        /// </summary>
        /// <param name="user">The user model with updated information.</param>
        /// <param name="id">The user ID.</param>
        /// <returns>Returns the updated user details.</returns>
        /// <response code="200">If the user was successfully updated</response>
        /// <response code="500">If the user was not found</response>
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
