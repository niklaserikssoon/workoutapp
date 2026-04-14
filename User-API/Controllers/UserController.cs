using Microsoft.AspNetCore.Mvc;
using User_API.Models;
using User_API.DTOs;
using User_API.Data;

namespace User_API.Controllers
{
    [Route("api/[controller]")]
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
                Email = userCreateDto.Email,
                PasswordHash = userCreateDto.password
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
    }
}
