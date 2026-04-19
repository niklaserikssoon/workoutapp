using Asp.Versioning;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User_API.Data;
using User_API.DTOs;
using User_API.Models;

namespace User_API.Controllers
{
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
