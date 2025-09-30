using EmployeeTracker.Models;

namespace EmployeeTracker.Services
{
    public interface IWorkSessionService
    {
        Task<WorkSession> ClockInAsync(int empId);
        Task<WorkSession> ClockOutAsync(int sessionId);
        Task<WorkSession> GetActiveSessionAsync(int empId);
    }

}
