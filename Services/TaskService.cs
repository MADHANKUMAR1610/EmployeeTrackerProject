using AutoMapper;
using EmployeeTracker.Dtos;
using EmployeeTracker.Models;
using EmployeeTracker.Repository;
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
            return _mapper.Map<EmpTaskDto>(task);
        }

        // ---------------- Get all tasks for employee ----------------
        public async Task<IEnumerable<EmpTaskDto>> GetTasksByEmployeeAsync(int empId)
        {
            var tasks = await _taskRepo.FindAsync(x => x.EmpId == empId || x.AssigneeId == empId);
            return _mapper.Map<IEnumerable<EmpTaskDto>>(tasks);
        }

        // ---------------- Get only pending tasks ----------------
        public async Task<IEnumerable<EmpTaskDto>> GetPendingTasksAsync(int empId)
        {
            var tasks = await _taskRepo.FindAsync(
                t => (t.EmpId == empId || t.AssigneeId == empId)
                  && t.Status == TaskStatus.Pending
            );
            return _mapper.Map<IEnumerable<EmpTaskDto>>(tasks);
        }

        // ---------------- Get completed tasks ----------------
        public async Task<IEnumerable<EmpTaskDto>> GetCompletedTasksAsync(int empId)
        {
            var tasks = await _taskRepo.FindAsync(
                t => (t.EmpId == empId || t.AssigneeId == empId)
                  && t.Status == TaskStatus.Completed
            );
            return _mapper.Map<IEnumerable<EmpTaskDto>>(tasks);
        }

        // ---------------- Mark task as completed ----------------
        public async Task<bool> CompleteTaskAsync(int taskId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);
            if (task == null) return false;

            task.Status = EmployeeTracker.Models.TaskStatus.Completed; // also add namespace to avoid ambiguity

            _taskRepo.Update(task);                 // ✅ no await here
            await _taskRepo.SaveChangesAsync();     // ✅ still async

            return true;
        }

        // ---------------- Get pending task count ----------------
        public async Task<int> GetPendingTaskCountAsync(int empId)
        {
            var tasks = await _taskRepo.FindAsync(
                t => t.EmpId == empId && t.Status == TaskStatus.Pending
            );
            return tasks.Count();
        }

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);
            if (task == null) return false;

            await _taskRepo.DeleteAsync(taskId);
            await _taskRepo.SaveChangesAsync();
            return true;
        }
        public async Task<EmpTaskDto> UpdateTaskAsync(int taskId, CreateEmpTaskDto dto)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);
            if (task == null) return null;

            _mapper.Map(dto, task);  // Map updated fields from DTO to existing task
            _taskRepo.Update(task);
            await _taskRepo.SaveChangesAsync();

            return _mapper.Map<EmpTaskDto>(task);
        }

    }
}