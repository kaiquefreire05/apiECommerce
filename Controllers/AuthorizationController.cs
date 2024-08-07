using ECommerceApi.DTOs;
using ECommerceApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUserRepository _rep;
        private readonly IConfiguration _config;
        public AuthorizationController(IUserRepository rep, IConfiguration config)
        {
            _rep = rep; 
            _config = config;
        }

        // Methods 
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] string username, [FromQuery] string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return BadRequest(new { message = "Invalid request payload." });
            }

            var user = await _rep.GetUserByUsernameAndPassword(username, password);
            if (user == null)
            {
                return Unauthorized("Invalid password or username.");
            }
            if (user.Role != Enums.UserRole.Admin)
            {
                return Forbid("User not authorized.");
            }

            var tk = GenerateTokenJWT();
            return Ok(new { token = tk });
        }

        private string GenerateTokenJWT()
        {
            var jwtSettings = _config.GetSection("jwt");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["secretkey"]);
            var expireTime = GetExpireTime();

            var issuer = jwtSettings["issuer"];
            var audience = jwtSettings["audience"];

            var key = new SymmetricSecurityKey(secretKey);
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("login", "admin"),
                new Claim("admin", "system admin")
            };

            var tk = new JwtSecurityToken
            (
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(expireTime),
                signingCredentials: credential
            );
            return new JwtSecurityTokenHandler().WriteToken(tk);
        }

        private int GetExpireTime()
        {
            // Getting value how string
            var jwtSettings = _config.GetSection("jwt");
            var expireTimeString = _config["expiretime"];

            // Try parse to int
            if (int.TryParse(expireTimeString, out int expireTime))
            {
                return expireTime;
            }
            // Exception
            throw new InvalidOperationException("Invalid expire time configuration.");
        }

    }
}
