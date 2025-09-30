using EmployeeTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceCalendarController : ControllerBase
    {
        private readonly IAttendanceCalendarService _service;

        public AttendanceCalendarController(IAttendanceCalendarService service)
        {
            _service = service;
        }

        // GET: api/attendancecalendar/calendar/1?month=9&year=2025
        [HttpGet("calendar/{empId}")]
        public async Task<IActionResult> GetCalendar(
            int empId,
            [FromQuery] int month,
            [FromQuery] int year)
        {
            if (empId <= 0 || month < 1 || month > 12 || year < 2000)
                return BadRequest("Invalid employee id, month, or year.");

            var result = await _service.GetCalendarAsync(empId, month, year);
            if (result == null)
                return NotFound($"No calendar found for employee {empId} in {month}/{year}");

            return Ok(result);
        }
    }
}
