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

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <response code="200">Returns the user</response>
        /// <response code="404">User not found</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.UserId,
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName
            });
        }

        /// <summary>
        /// Updates an existing user's details.
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <param name="dto">Updated user details</param>
        /// <response code="204">Update successful</response>
        /// <response code="400">Invalid input</response>
        /// <response code="404">User not found</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] UpdateUserDTO dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <response code="204">Deletion successful</response>
        /// <response code="404">User not found</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
