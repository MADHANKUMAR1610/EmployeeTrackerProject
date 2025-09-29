using EmployeeTracker.Dtos;
using EmployeeTracker.Models;

namespace EmployeeTracker.Services
{
    public interface ITaskService
    {
        // Create new task
        Task<EmpTaskDto> CreateTaskAsync(CreateEmpTaskDto dto);

        // Get all tasks for employee
        Task<IEnumerable<EmpTaskDto>> GetTasksByEmployeeAsync(int empId);

        // Get pending tasks
        Task<IEnumerable<EmpTaskDto>> GetPendingTasksAsync(int empId);

        // Get completed tasks
        Task<IEnumerable<EmpTaskDto>> GetCompletedTasksAsync(int empId);

        // Mark task as completed
        Task<bool> CompleteTaskAsync(int taskId);
        Task<int> GetPendingTaskCountAsync(int empId);
    }
}
