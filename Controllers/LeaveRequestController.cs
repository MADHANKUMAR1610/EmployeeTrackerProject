using EmployeeTracker.Models;
using EmployeeTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly LeaveRequestService _service;
        public LeaveRequestController(LeaveRequestService service) => _service = service;

        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());
        [HttpGet("{id}")] public async Task<IActionResult> Get(int id) => Ok(await _service.GetByIdAsync(id));
        [HttpGet("employee/{employeeId}")] public async Task<IActionResult> GetByEmployee(int employeeId) => Ok(await _service.GetByEmployeeAsync(employeeId));

        // Submit leave request (Pending)
        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromBody] LeaveRequest lr)
        {
            var saved = await _service.SubmitLeaveAsync(lr);
            return CreatedAtAction(nameof(Get), new { id = saved.LeaveRequestId }, saved);
        }

        // Approve leave (deducts from balance)
        [HttpPost("approve/{leaveId}")]
        public async Task<IActionResult> Approve(int leaveId)
        {
            var (ok, msg) = await _service.ApproveLeaveAsync(leaveId);
            if (!ok) return BadRequest(msg);
            return Ok(new { Message = msg });
        }

        // Cancel leave (reverts balance if previously approved)
        [HttpPost("cancel/{leaveId}")]
        public async Task<IActionResult> Cancel(int leaveId)
        {
            var ok = await _service.CancelLeaveAsync(leaveId);
            if (!ok) return NotFound();
            return Ok(new { Message = "Cancelled" });
        }

        // Get leave balances for dashboard/attendance page
        [HttpGet("balances/{employeeId}")]
        public async Task<IActionResult> GetBalances(int employeeId)
        {
            var balances = await _service.GetBalancesAsync(employeeId);
            // Return a simple DTO for UI
            var dto = balances.Select(b => new {
                LeaveType = b.LeaveType.ToString(),
                TotalLeaves = b.TotalLeaves,
                UsedLeaves = b.UsedLeaves,
                RemainingLeaves = b.TotalLeaves - b.UsedLeaves
            });
            return Ok(dto);
        }
    }
}
