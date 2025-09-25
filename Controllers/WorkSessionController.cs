using EmployeeTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkSessionController : ControllerBase
    {
        private readonly WorkSessionService _service;
        public WorkSessionController(WorkSessionService service) => _service = service;

        // Start session (clock-in)
        // Example: POST /api/worksessions/start
        // Body optional, but we will take employeeId from token optionally
        [HttpPost("start")]
        public async Task<IActionResult> StartSession([FromBody] StartSessionDto dto)
        {
            var employeeId = dto?.EmployeeId ?? int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var session = await _service.StartSessionAsync(employeeId);
            // When user clicks "Login" in UI this endpoint locks that time as LoginTime.
            return Ok(session);
        }

        // End session (clock-out)
        // Example: POST /api/worksessions/end/{sessionId}
        [HttpPost("end/{sessionId}")]
        public async Task<IActionResult> EndSession(int sessionId)
        {
            var session = await _service.EndSessionAsync(sessionId);
            if (session == null) return NotFound("Session not found or already ended.");
            // When user clicks "Logout" in UI this endpoint locks LogoutTime and calculates total hours
            return Ok(session);
        }

        [HttpGet("employee/{employeeId}")] public async Task<IActionResult> GetByEmployee(int employeeId) => Ok(await _service.GetByEmployeeAsync(employeeId));
        [HttpGet("{id}")] public async Task<IActionResult> Get(int id) => Ok(await _service.GetByIdAsync(id));

        public class StartSessionDto { public int? EmployeeId { get; set; } }
    }
}
