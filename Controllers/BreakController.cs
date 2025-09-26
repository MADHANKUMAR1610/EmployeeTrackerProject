using EmployeeTracker.Services;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BreakController : ControllerBase
    {
        private readonly IBreakService _bs;
        public BreakController(IBreakService bs) => _bs = bs;

        [HttpPost("start/{empId}")]
        public async Task<IActionResult> Start(int empId)
        {
            var b = await _bs.StartBreakAsync(empId);
            if (b == null) return BadRequest("No active session found.");
            return Ok(b);
        }

        [HttpPost("end/{empId}")]
        public async Task<IActionResult> End(int empId)
        {
            var b = await _bs.EndBreakAsync(empId);
            if (b == null) return BadRequest("No active break found.");
            return Ok(b);
        }
    }
}

