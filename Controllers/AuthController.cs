using EmployeeTracker.Services;
using EmployeeTracker.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
                return BadRequest("Invalid request body");

            var user = await _authService.LoginAsync(loginDto.Email, loginDto.Password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            return Ok(user);
        }
    }
}
