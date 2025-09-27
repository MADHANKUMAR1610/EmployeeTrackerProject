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
        private readonly IWorkSessionService _ws;
        public WorkSessionController(IWorkSessionService ws) => _ws = ws;

        [HttpPost("clockin/{empId}")]
        public async Task<IActionResult> ClockIn(int empId)
        {
            var session = await _ws.ClockInAsync(empId);
            if (session == null) return BadRequest("Unable to create session");
            return Ok(session);
        }

        [HttpPost("logout/{empId}")]
        public async Task<IActionResult> Logout(int empId)
        {
            var session = await _ws.ClockOutAsync(empId);
            if (session == null) return BadRequest("No active session.");
            return Ok(session);
        }

        [HttpGet("active/{empId}")]
        public async Task<IActionResult> GetActive(int empId)
        {
            var session = await _ws.GetActiveSessionAsync(empId);
            if (session == null) return NotFound();
            return Ok(session);
        }
    }
}
