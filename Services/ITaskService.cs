using EmployeeTracker.Models;

namespace EmployeeTracker.Services
{
    public interface ITaskService
    {
        Task<EmpTask> CreateTaskAsync(EmpTask t);
        Task<IEnumerable<EmpTask>> GetByEmpAsync(int empId);
    }
}
