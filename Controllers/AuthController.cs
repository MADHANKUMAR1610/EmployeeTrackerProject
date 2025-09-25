using EmployeeTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // ✅ Only Login Endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _authService.LoginAsync(loginDto.Email!, loginDto.Password!);

            if (token == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { Token = token });
        }
    }

    // DTO for login request
    public class LoginDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}

