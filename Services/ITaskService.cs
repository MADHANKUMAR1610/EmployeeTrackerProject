using EmployeeTracker.Dtos;

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

        // Get pending task count
        Task<int> GetPendingTaskCountAsync(int empId);

        Task<bool> DeleteTaskAsync(int taskId);
        // Update an existing task
        Task<EmpTaskDto> UpdateTaskAsync(int taskId, CreateEmpTaskDto dto);
       

    }
}
