using AutoMapper;
using EmployeeTracker.Datas;
using EmployeeTracker.Dtos;
using EmployeeTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracker.Services
{
    public class TaskService : ITaskService
    {
        private readonly EmployeeTrackerDbContext _ctx;
        private readonly IMapper _mapper;

        public TaskService(EmployeeTrackerDbContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        // Create a new task
        public async Task<EmpTaskDto> CreateTaskAsync(CreateEmpTaskDto dto)
        {
            var task = _mapper.Map<EmpTask>(dto);
            _ctx.EmpTask.Add(task);
            await _ctx.SaveChangesAsync();
            return _mapper.Map<EmpTaskDto>(task);
        }

        // Get all tasks for employee (assigned or created by)
        public async Task<IEnumerable<EmpTaskDto>> GetTasksByEmployeeAsync(int empId)
        {
            var tasks = await _ctx.EmpTask
                .Where(x => x.EmpId == empId || x.AssigneeId == empId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EmpTaskDto>>(tasks);
        }

        // Get only pending tasks
        public async Task<IEnumerable<EmpTaskDto>> GetPendingTasksAsync(int empId)
        {
            var tasks = await _ctx.EmpTask
                .Where(t => (t.EmpId == empId || t.AssigneeId == empId)
                         && t.Status == Models.TaskStatus.Pending)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EmpTaskDto>>(tasks);
        }

        // Get completed tasks
        public async Task<IEnumerable<EmpTaskDto>> GetCompletedTasksAsync(int empId)
        {
            var tasks = await _ctx.EmpTask
                .Where(t => (t.EmpId == empId || t.AssigneeId == empId)
                         && t.Status == Models.TaskStatus.Completed)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EmpTaskDto>>(tasks);
        }

        // Mark task as completed
        public async Task<bool> CompleteTaskAsync(int taskId)
        {
            var task = await _ctx.EmpTask.FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null) return false;

            task.Status = Models.TaskStatus.Completed;
            _ctx.EmpTask.Update(task);
            await _ctx.SaveChangesAsync();
            return true;
        }
        public async Task<int> GetPendingTaskCountAsync(int empId)
        {
            return await _ctx.EmpTask
                .CountAsync(t => t.EmpId == empId && t.Status == Models.TaskStatus.Pending);
        }

    }
}
