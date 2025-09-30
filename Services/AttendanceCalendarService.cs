using EmployeeTracker.Dtos;
using EmployeeTracker.Models;
using EmployeeTracker.Repository;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracker.Services
{
    public class AttendanceCalendarService : IAttendanceCalendarService
    {
        private readonly IGenericRepository<WorkSession> _workSessionRepo;
        private readonly IGenericRepository<LeaveRequest> _leaveRequestRepo;

        public AttendanceCalendarService(
            IGenericRepository<WorkSession> workSessionRepo,
            IGenericRepository<LeaveRequest> leaveRequestRepo)
        {
            _workSessionRepo = workSessionRepo;
            _leaveRequestRepo = leaveRequestRepo;
        }

        public async Task<AttendanceCalendarDto> GetCalendarAsync(int empId, int month, int year)
        {
            var days = new List<AttandanceCalenderDto>();

            // ✅ Get total days in the requested month
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var startDate = new DateTime(year, month, 1);
            var endDate = new DateTime(year, month, daysInMonth);

            // ✅ Get employee sessions (logins) for the month
            var sessions = await _workSessionRepo.FindAsync(s =>
                s.EmpId == empId &&
                s.LoginTime.Month == month &&
                s.LoginTime.Year == year
            );

            // ✅ Get approved leave requests for the employee in this month
            var leaveRequests = await _leaveRequestRepo.FindAsync(lr =>
                lr.EmpId == empId &&
                lr.Status == LeaveStatus.Approved &&   // Only approved leaves
                lr.StartDate.Date <= endDate &&
                lr.EndDate.Date >= startDate
            );

            // ✅ Build attendance per day
            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(year, month, day);
                string status = "NotMarked";

                // Weekend → Holiday
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    status = "Holiday";
                }
                // Present → If session exists
                else if (sessions.Any(s => s.LoginTime.Date == date))
                {
                    status = "Present";
                }
                // Absent → If leave is approved for that day
                else if (leaveRequests.Any(lr => lr.StartDate.Date <= date && lr.EndDate.Date >= date))
                {
                    status = "Absent";
                }
<<<<<<< HEAD
=======

>>>>>>> 86c4747b5b8c3d99cb586e9d128afa4e7b90ac53
                days.Add(new AttandanceCalenderDto
                {
                    Date = date,
                    Status = status
                });
            }

            return new AttendanceCalendarDto
            {
                EmpId = empId,
                Month = month,
                Year = year,
                Days = days
            };
        }
    }
}
