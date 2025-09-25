using EmployeeTracker.Models;
using EmployeeTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly AttendanceService _service;
        public AttendanceController(AttendanceService service) => _service = service;

        // Get all attendance records
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        // Get attendance for employee
        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployee(int employeeId) => Ok(await _service.GetByEmployeeAsync(employeeId));

        // Create an attendance record (for example, when employee clocks in/out you may create/update record)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Attendance a)
        {
            await _service.AddAsync(a);
            return CreatedAtAction(nameof(GetByEmployee), new { employeeId = a.EmployeeId }, a);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Attendance a) { if (id != a.AttendanceId) return BadRequest(); await _service.UpdateAsync(a); return NoContent(); }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) { await _service.DeleteAsync(id); return NoContent(); }
    }
}
