using AutoMapper;
using EmployeeTracker.Dtos;
using EmployeeTracker.Models;
using EmployeeTracker.Repository;
using Microsoft.EntityFrameworkCore;
using TaskStatus = EmployeeTracker.Models.TaskStatus;

namespace EmployeeTracker.Services
{
    public class TaskService : ITaskService
    {
        private readonly IGenericRepository<EmpTask> _taskRepo;
        private readonly IMapper _mapper;

        public TaskService(IGenericRepository<EmpTask> taskRepo, IMapper mapper)
        {
            _taskRepo = taskRepo;
            _mapper = mapper;
        }

        // ---------------- Create a new task ----------------
        public async Task<EmpTaskDto> CreateTaskAsync(CreateEmpTaskDto dto)
        {
            var task = _mapper.Map<EmpTask>(dto);
            await _taskRepo.AddAsync(task);
            await _taskRepo.SaveChangesAsync();

            // include Assignee info after save
            var savedTask = await _taskRepo
                .Query()
                .Include(t => t.Assignee)
                .FirstOrDefaultAsync(t => t.Id == task.Id);

            return _mapper.Map<EmpTaskDto>(savedTask);
        }

        // ---------------- Get all tasks for employee ----------------
        public async Task<IEnumerable<EmpTaskDto>> GetTasksByEmployeeAsync(int empId)
        {
            var tasks = await _taskRepo
                .Query()
                .Where(t => t.EmpId == empId || t.AssigneeId == empId)
                .Include(t => t.Assignee)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EmpTaskDto>>(tasks);
        }

        // ---------------- Get only pending tasks ----------------
        public async Task<IEnumerable<EmpTaskDto>> GetPendingTasksAsync(int empId)
        {
            var tasks = await _taskRepo
                .Query()
                .Where(t => (t.EmpId == empId || t.AssigneeId == empId) && t.Status == TaskStatus.Pending)
                .Include(t => t.Assignee)
                .ToListAsync();

            // AutoMapper will automatically map Assignee.Name → AssigneeName
            return _mapper.Map<IEnumerable<EmpTaskDto>>(tasks);
        }

        // ---------------- Get completed tasks ----------------
        public async Task<IEnumerable<EmpTaskDto>> GetCompletedTasksAsync(int empId)
        {
            var tasks = await _taskRepo
                .Query()
                .Where(t => (t.EmpId == empId || t.AssigneeId == empId) && t.Status == TaskStatus.Completed)
                .Include(t => t.Assignee)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EmpTaskDto>>(tasks);
        }

        // ---------------- Mark task as completed ----------------
        public async Task<bool> CompleteTaskAsync(int taskId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);
            if (task == null) return false;

            task.Status = TaskStatus.Completed;

            _taskRepo.Update(task);
            await _taskRepo.SaveChangesAsync();
            return true;
        }

        // ---------------- Get pending task count ----------------
        public async Task<int> GetPendingTaskCountAsync(int empId)
        {
            return await _taskRepo
                .Query()
                .CountAsync(t => (t.EmpId == empId || t.AssigneeId == empId) && t.Status == TaskStatus.Pending);
        }

        // ---------------- Delete a task ----------------
        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);
            if (task == null) return false;

            await _taskRepo.DeleteAsync(taskId);
            await _taskRepo.SaveChangesAsync();
            return true;
        }

        // ---------------- Update a task ----------------
        public async Task<EmpTaskDto?> UpdateTaskAsync(int taskId, CreateEmpTaskDto dto)
        {
            var task = await _taskRepo
                .Query()
                .Include(t => t.Assignee)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null) return null;

            _mapper.Map(dto, task); // AutoMapper handles mapping DTO → Model (with enums)
            _taskRepo.Update(task);
            await _taskRepo.SaveChangesAsync();

            return _mapper.Map<EmpTaskDto>(task);
        }
    }
}
