using EmployeeTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AttandanceCalenderController : ControllerBase
    {
        private readonly IAttendanceCalendarService _service; // 👈 Add this

        public AttandanceCalenderController(IAttendanceCalendarService service)
        {
            _service = service;
        }

        [HttpGet("calendar/{empId}")]
        public async Task<IActionResult> GetCalendar(int empId, int month, int year)
        {
            var result = await _service.GetCalendarAsync(empId, month, year);
            return Ok(result);
        }
    }
}