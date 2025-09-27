using EmployeeTracker.Dtos;

namespace EmployeeTracker.Services
{
    public interface IAttendanceCalendarService
    {
        Task<AttendanceCalendarDto> GetCalendarAsync(int empId, int month, int year);
    }
}
