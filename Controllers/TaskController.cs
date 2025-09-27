using EmployeeTracker.Models;
using EmployeeTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _ts;
        public TaskController(ITaskService ts) => _ts = ts;

        [HttpPost]
        public async Task<IActionResult> Create(EmpTask t) => Ok(await _ts.CreateTaskAsync(t));

        [HttpGet("byemp/{empId}")]
        public async Task<IActionResult> ByEmp(int empId) => Ok(await _ts.GetByEmpAsync(empId));
    }
}
