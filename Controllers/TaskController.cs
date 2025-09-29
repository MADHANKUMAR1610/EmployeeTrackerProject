using AutoMapper;
using EmployeeTracker.Datas;
using EmployeeTracker.Dtos;
using EmployeeTracker.Models;
using EmployeeTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // Create a new task
        [HttpPost]
        public async Task<ActionResult<EmpTaskDto>> CreateTask(CreateEmpTaskDto dto)
        {
            var task = await _taskService.CreateTaskAsync(dto);
            return Ok(task); // already mapped in service
        }

        // Get all tasks assigned to or created by employee
        [HttpGet("by-employee/{empId}")]
        public async Task<ActionResult<IEnumerable<EmpTaskDto>>> GetTasksByEmployee(int empId)
        {
            var tasks = await _taskService.GetTasksByEmployeeAsync(empId);
            return Ok(tasks);
        }

        // Dashboard - get only pending tasks
        [HttpGet("pending/{empId}")]
        public async Task<IActionResult> GetPendingTasks(int empId)
        {
            var tasks = await _taskService.GetPendingTasksAsync(empId);
            return Ok(tasks);
        }

        // Task page - get completed tasks
        [HttpGet("completed/{empId}")]
        public async Task<IActionResult> GetCompletedTasks(int empId)
        {
            var tasks = await _taskService.GetCompletedTasksAsync(empId);
            return Ok(tasks);
        }

        // Update task status -> completed
        [HttpPut("complete/{taskId}")]
        public async Task<IActionResult> CompleteTask(int taskId)
        {
            var result = await _taskService.CompleteTaskAsync(taskId);
            if (!result) return NotFound("Task not found");

            return Ok("Task marked as completed");

        }
        // Get pending task count for dashboard
        [HttpGet("pending/count/{empId}")]
        public async Task<IActionResult> GetPendingTaskCount(int empId)
        {
            var count = await _taskService.GetPendingTaskCountAsync(empId);
            return Ok(count);
        }

    }
}