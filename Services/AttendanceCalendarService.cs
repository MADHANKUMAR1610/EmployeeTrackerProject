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
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1).AddDays(-1);

            var attendances = await _ctx.WorkSessions
           .Where(ws => ws.EmpId == empId && ws.LoginTime >= start && ws.LoginTime <= end)
            .ToListAsync();

            var leaves = await _ctx.LeaveRequests
                .Where(l => l.EmpId == empId && l.Status == LeaveStatus.Approved
                         && l.StartDate <= end && l.EndDate >= start)
                .ToListAsync();

            var days = new List<AttandanceCalenderDto>();

            for (var date = start; date <= end; date = date.AddDays(1))
            {
                // Default → NotMarked instead of Absent
                var status = "NotMarked";

                // If employee has attendance record → Present
                if (attendances.Any(a => a.LoginTime.Date == date.Date))
                    status = "Present";

                // If employee is on leave → Leave (overrides present if overlapping)
                if (leaves.Any(l => l.StartDate <= date && l.EndDate >= date))
                    status = "Absent";

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
