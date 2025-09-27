using EmployeeTracker.Models;
using EmployeeTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _ls;
        public LeaveController(ILeaveService ls) => _ls = ls;

        [HttpPost("apply")]
        public async Task<IActionResult> Apply(LeaveRequest request)
        {
            var r = await _ls.ApplyLeaveAsync(request);
            return Ok(r);
        }

        [HttpPost("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var r = await _ls.ApproveLeaveAsync(id);
            if (r == null) return NotFound();
            return Ok(r);
        }

        [HttpGet("byemp/{empId}")]
        public async Task<IActionResult> ByEmp(int empId) => Ok(await _ls.GetByEmpAsync(empId));
    }
}

