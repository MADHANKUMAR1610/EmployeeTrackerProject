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
        private readonly IGenericRepository<Employee> _employeeRepo;
        private readonly IMapper _mapper;

        public TaskService(
            IGenericRepository<EmpTask> taskRepo,
            IGenericRepository<Employee> employeeRepo,
            IMapper mapper)
        {
            _taskRepo = taskRepo;
            _employeeRepo = employeeRepo;
            _mapper = mapper;
        }

        // ---------------- Create a new task ----------------
        public async Task<EmpTaskDto> CreateTaskAsync(CreateEmpTaskDto dto)
        {
            // Check if creator exists
            var employee = await _employeeRepo.GetByIdAsync(dto.EmpId);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with Id {dto.EmpId} not found.");

            var entity = _mapper.Map<EmpTask>(dto);

            // Map Assignee if provided
            if (!string.IsNullOrEmpty(dto.AssigneeName))
            {
                var assignee = await _employeeRepo
                    .Query()
                    .FirstOrDefaultAsync(e => e.Name == dto.AssigneeName);

                if (assignee != null)
                    entity.AssigneeId = assignee.Id;
                else
                    throw new KeyNotFoundException($"Employee '{dto.AssigneeName}' not found.");
            }

            entity.EmpId = dto.EmpId;

            await _taskRepo.AddAsync(entity);

            var savedTask = await _taskRepo
                .Query()
                .Include(t => t.Assignee)
                .FirstOrDefaultAsync(t => t.Id == entity.Id);

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
            var entity = await _taskRepo.GetByIdAsync(taskId);
            if (entity == null) return false;

            entity.Status = TaskStatus.Completed;
            await _taskRepo.UpdateAsync(entity);

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
            var entity = await _taskRepo.GetByIdAsync(taskId);
            if (entity == null) return false;

            await _taskRepo.DeleteAsync(taskId);
            return true;
        }

        // ---------------- Update a task ----------------
        public async Task<EmpTaskDto> UpdateTaskAsync(int taskId, UpdateEmpTaskDto dto)
        {
            var entity = await _taskRepo
                .Query()
                .Include(t => t.Assignee)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (entity == null) return null;

            _mapper.Map(dto, entity);
            await _taskRepo.UpdateAsync(entity);

            return _mapper.Map<EmpTaskDto>(entity);
        }
    }
}
