using EmployeeTracker.Models;

namespace EmployeeTracker.Services
{
    public interface IBreakService
    {
        Task<Break> StartBreakAsync(int empId);
        Task<Break> EndBreakAsync(int empId);
    }
}
