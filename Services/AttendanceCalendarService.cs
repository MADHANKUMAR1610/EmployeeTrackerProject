using EmployeeTracker.Datas;
using EmployeeTracker.Dtos;
using EmployeeTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracker.Services
{
    public class AttendanceCalendarService : IAttendanceCalendarService
    {
        private readonly EmployeeTrackerDbContext _ctx;

        public AttendanceCalendarService(EmployeeTrackerDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<AttendanceCalendarDto> GetCalendarAsync(int empId, int month, int year)
        {
            var days = new List<AttandanceCalenderDto>();

            // Get total days in month
            int daysInMonth = DateTime.DaysInMonth(year, month);

            // Get employee sessions (logins)
            var sessions = await _ctx.WorkSessions
                .Where(s => s.EmpId == empId &&
                            s.LoginTime.Month == month &&
                            s.LoginTime.Year == year)
                .ToListAsync();

            // Get employee leaves
            var leaveRequests = await _ctx.LeaveRequests
                .Where(lr => lr.EmpId == empId &&
                             lr.StartDate.Date <= new DateTime(year, month, daysInMonth) &&
                             lr.EndDate.Date >= new DateTime(year, month, 1))
                .ToListAsync();

            // Build attendance per day
            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(year, month, day);
                string status = "NotMarked";

                // ✅ Weekends as Holiday
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    status = "Holiday";
                }
                // ✅ Present if session exists
                else if (sessions.Any(s => s.LoginTime.Date == date))
                {
                    status = "Present";
                }
                // ✅ Absent if leave approved
                else if (leaveRequests.Any(lr => lr.StartDate.Date <= date && lr.EndDate.Date >= date))
                {
                    status = "Absent";
                }
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

