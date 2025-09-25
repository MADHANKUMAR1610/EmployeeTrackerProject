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
        private readonly TaskService _service;
        public TaskController(TaskService service) => _service = service;

        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());
        [HttpGet("{id}")] public async Task<IActionResult> Get(int id) => Ok(await _service.GetByIdAsync(id));
        [HttpGet("employee/{employeeId}")] public async Task<IActionResult> GetByEmployee(int employeeId) => Ok(await _service.GetByEmployeeAsync(employeeId));

        [HttpPost] public async Task<IActionResult> Create(EmpTask task) { await _service.AddAsync(task); return CreatedAtAction(nameof(Get), new { id = task.TaskId }, task); }
        [HttpPut("{id}")] public async Task<IActionResult> Update(int id, EmpTask task) { if (id != task.TaskId) return BadRequest(); await _service.UpdateAsync( task); return NoContent(); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { await _service.DeleteAsync(id); return NoContent(); }

        // Mark done (extra endpoint)
        [HttpPost("markdone/{taskId}")] public async Task<IActionResult> MarkDone(int taskId) { var ok = await _service.MarkDoneAsync(taskId); if (!ok) return NotFound(); return Ok(); }

        // Change priority
        [HttpPost("priority/{taskId}")] public async Task<IActionResult> ChangePriority(int taskId, [FromBody] string priority) { var ok = await _service.ChangePriorityAsync(taskId, priority); if (!ok) return NotFound(); return Ok(); }
    }
}
