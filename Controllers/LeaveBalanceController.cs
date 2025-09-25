using EmployeeTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LeaveBalanceController : ControllerBase
    {
        private readonly LeaveBalanceService _leaveBalanceService;

        public LeaveBalanceController(LeaveBalanceService leaveBalanceService)
        {
            _leaveBalanceService = leaveBalanceService;
        }

        // ✅ Get balances (Attendance Page)
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetBalances(int employeeId)
        {
            var balances = await _leaveBalanceService.GetLeaveBalances(employeeId);
            return Ok(balances.Select(b => new
            {
                LeaveType = b.LeaveType.ToString(),
                Total = b.TotalLeaves,
                Used = b.UsedLeaves,
                Remaining = b.RemainingLeaves
            }));
        }
    }
}
