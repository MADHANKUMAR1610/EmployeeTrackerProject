using EmployeeTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BreakController : ControllerBase
    {
        private readonly BreakService _service;
        public BreakController(BreakService service) => _service = service;

        // Start break for a session (employee clicked "Take Break")
        [HttpPost("start/{sessionId}")]
        public async Task<IActionResult> StartBreak(int sessionId, [FromBody] StartBreakDto dto)
        {
            var br = await _service.StartBreakAsync(sessionId);
            // UI should record returned BreakId so it can call end later.
            return Ok(br);
        }

        // End break (employee clicked "Close Break")
        [HttpPost("end/{breakId}")]
        public async Task<IActionResult> EndBreak(int breakId)
        {
            var br = await _service.EndBreakAsync(breakId);
            if (br == null) return NotFound("Break not found or already ended.");
            // This sets BreakEndTime and BreakDurationHours.
            return Ok(br);
        }

        public class StartBreakDto { public string? Type { get; set; } }
    }
}

