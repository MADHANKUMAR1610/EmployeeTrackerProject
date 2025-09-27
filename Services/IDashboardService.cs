using EmployeeTracker.Dtos;

namespace EmployeeTracker.Services
{
    public interface IDashboardService
    {
        Task<DashBoardDto> GetDashboardAsync(int empId);
    }
}
