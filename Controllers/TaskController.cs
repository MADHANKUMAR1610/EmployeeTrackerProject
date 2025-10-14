using AutoMapper;
using EmployeeTracker.Dtos;
using EmployeeTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;

        public TaskController(ITaskService taskService, IMapper mapper)
        {
            _taskService = taskService;
            _mapper = mapper;
        }

        // ---------------- Create a new task ----------------
        [HttpPost]
        public async Task<ActionResult<EmpTaskDto>> CreateTask([FromBody] CreateEmpTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = await _taskService.CreateTaskAsync(dto);
            return CreatedAtAction(nameof(GetTaskById), new { taskId = task.Id }, task);
        }

        // ---------------- Get single task by Id (for task details page) ----------------
        [HttpGet("{taskId}")]
        public async Task<ActionResult<EmpTaskDto>> GetTaskById(int taskId)
        {
            var tasks = await _taskService.GetTasksByEmployeeAsync(0); // fallback if service has no GetById
            var task = tasks.FirstOrDefault(t => t.Id == taskId);

            if (task == null)
                return NotFound("Task not found");

            return Ok(task);
        }

        // ---------------- Get all tasks for an employee ----------------
        [HttpGet("by-employee/{empId}")]
        public async Task<ActionResult<IEnumerable<EmpTaskDto>>> GetTasksByEmployee(int empId)
        {
            var tasks = await _taskService.GetTasksByEmployeeAsync(empId);
            if (tasks == null || !tasks.Any())
                return NotFound("No tasks found for this employee");

            return Ok(tasks);
        }

        // ---------------- Get only pending tasks ----------------
        [HttpGet("pending/{empId}")]
        public async Task<ActionResult<IEnumerable<EmpTaskDto>>> GetPendingTasks(int empId)
        {
            var tasks = await _taskService.GetPendingTasksAsync(empId);
            if (tasks == null || !tasks.Any())
                return Ok(new List<EmpTaskDto>()); // return empty list instead of null

            return Ok(tasks);
        }

        // ---------------- Get only completed tasks ----------------
        [HttpGet("completed/{Emp_id}")]
        public async Task<ActionResult<IEnumerable<EmpTaskDto>>> GetCompletedTasks(int empId)
        {
            var tasks = await _taskService.GetCompletedTasksAsync(empId);
            return Ok(tasks);
        }

        // ---------------- Mark task as completed ----------------
        [HttpPut("complete/{taskId}")]
        public async Task<IActionResult> CompleteTask(int taskId)
        {
            var result = await _taskService.CompleteTaskAsync(taskId);
            if (!result)
                return NotFound("Task not found");

            return Ok(new { Message = "Task marked as completed successfully" });
        }

        // ---------------- Get pending task count (for dashboard) ----------------
        [HttpGet("pending/count/Emp_id")]
        public async Task<IActionResult> GetPendingTaskCount(int empId)
        {
            var count = await _taskService.GetPendingTaskCountAsync(empId);
            return Ok(new { PendingCount = count });
        }

        // ---------------- Update existing task ----------------
        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask(int taskId, [FromBody] CreateEmpTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedTask = await _taskService.UpdateTaskAsync(taskId, dto);
            if (updatedTask == null)
                return NotFound("Task not found");

            return Ok(updatedTask);
        }

        // ---------------- Delete a task ----------------
        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var result = await _taskService.DeleteTaskAsync(taskId);
            if (!result)
                return NotFound("Task not found");

            return Ok(new { Message = "Task deleted successfully" });
        }
    }
}
