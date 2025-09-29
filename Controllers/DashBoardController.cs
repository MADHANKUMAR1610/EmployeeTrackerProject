using EmployeeTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class DashBoardController : ControllerBase
    {
        private readonly IDashboardService _service;
        public DashBoardController(IDashboardService service) => _service = service;

        [HttpGet("{empId}")]
        public async Task<IActionResult> Get(int empId)
        {
            var result = await _service.GetDashboardAsync(empId);
            return Ok(result);

        }
    }
}
