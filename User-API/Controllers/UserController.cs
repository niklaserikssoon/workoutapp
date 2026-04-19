using Asp.Versioning;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User_API.Data;
using User_API.DTOs;
using User_API.Models;

namespace User_API.Controllers
{
    /// <summary>
    /// Manages user registration and authentication.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;

        public UserController(UserDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <param name="userCreateDto">User registration details</param>
        /// <response code="200">User created successfully</response>
        /// <response code="400">Invalid input</response>
        [HttpPost("register")]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDTO userCreateDto)
        {
            var user = new User
            {
                FirstName = userCreateDto.FirstName,
                LastName = userCreateDto.LastName,
                UserName = userCreateDto.UserName,
                Email = userCreateDto.Email,
                PasswordHash = userCreateDto.password
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        /// <summary>
        /// Authenticates a user with username and password.
        /// </summary>
        /// <param name="dto">Login credentials</param>
        /// <response code="200">Login successful, returns user info</response>
        /// <response code="400">Missing credentials</response>
        /// <response code="401">Invalid username or password</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest();

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == dto.UserName);

            if (user == null || user.PasswordHash != dto.Password)
                return Unauthorized(new { message = "Wrong username or password." });

            return Ok(new
            {
                user.UserId,
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName
            });
        }
    }
}
