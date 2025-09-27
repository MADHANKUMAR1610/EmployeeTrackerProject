using EmployeeTracker.Models;

namespace EmployeeTracker.Services
{
    public interface IAttendanceService
    {
        Task<IEnumerable<Attendance>> GetAttendanceByEmpAsync(int empId);
    }
}
