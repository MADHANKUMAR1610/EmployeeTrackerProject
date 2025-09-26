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

        // ✅ Get all tasks with Creator + Assignee details
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _service.GetTasksWithAssigneeAsync();
            return Ok(tasks);
        }

        // ✅ Get a single task
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var task = await _service.GetByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        // ✅ Get tasks created by a specific employee
        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployee(int employeeId)
        {
            var tasks = await _service.GetByEmployeeAsync(employeeId);
            return Ok(tasks);
        }

        // ✅ Create new task (can also set AssigneeId & Tag here)
        [HttpPost]
        public async Task<IActionResult> Create(EmpTask task)
        {
            await _service.AddAsync(task);
            return CreatedAtAction(nameof(Get), new { id = task.TaskId }, task);
        }

        // ✅ Update task
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EmpTask task)
        {
            if (id != task.TaskId) return BadRequest();
            await _service.UpdateAsync(task);
            return NoContent();
        }

        // ✅ Delete task
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        // ✅ Mark task as completed
        [HttpPost("markdone/{taskId}")]
        public async Task<IActionResult> MarkDone(int taskId)
        {
            var ok = await _service.MarkDoneAsync(taskId);
            if (!ok) return NotFound("Task not found");
            return Ok("Task marked as completed");
        }

        // ✅ Change priority of a task
        [HttpPost("priority/{taskId}")]
        public async Task<IActionResult> ChangePriority(int taskId, [FromBody] string priority)
        {
            var ok = await _service.ChangePriorityAsync(taskId, priority);
            if (!ok) return NotFound("Task not found");
            return Ok($"Priority updated to {priority}");
        }

        // ✅ Assign task to another employee
        [HttpPut("{taskId}/assign/{assigneeId}")]
        public async Task<IActionResult> AssignTask(int taskId, int assigneeId)
        {
            var ok = await _service.AssignTaskAsync(taskId, assigneeId);
            if (!ok) return NotFound("Task or Employee not found");
            return Ok("Task assigned successfully");
        }
    }
}
