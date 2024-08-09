using ECommerceApi.Repositories.Interfaces;
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

        /// <summary>
        /// Authenticates a user and generates a JWT token if credentials are valid.
        /// </summary>
        /// <param name="username">The username of the user attempting to log in.</param>
        /// <param name="password">The password of the user attempting to log in.</param>
        /// <returns>Returns a JWT token if authentication is successful.</returns>
        /// <response code="200">Returns the JWT token</response>
        /// <response code="400">If the username or password is not provided</response>
        /// <response code="401">If the username or password is incorrect</response>
        /// <response code="403">If the user is not authorized</response>
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

        /// <summary>
        /// Generates a JWT token based on predefined settings.
        /// </summary>
        /// <returns>Returns a JWT token as a string.</returns>
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

        /// <summary>
        /// Retrieves the expiration time for the JWT token from the configuration.
        /// </summary>
        /// <returns>Returns the expiration time in hours as an integer.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the expire time configuration is invalid.</exception>
        private int GetExpireTime()
        {
            // Getting value as string
            var jwtSettings = _config.GetSection("jwt");
            var expireTimeString = jwtSettings["expiretime"];

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
