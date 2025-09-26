using EmployeeTracker.Datas;
using EmployeeTracker.Models;
using EmployeeTracker.Repository;
using Microsoft.EntityFrameworkCore;
using System;

namespace EmployeeTracker.Services
{
    public class WorkSessionService : IWorkSessionService
    {
        private readonly EmployeeTrackerDbContext _ctx;
        public WorkSessionService(EmployeeTrackerDbContext ctx) => _ctx = ctx;

        public async Task<WorkSession> ClockInAsync(int empId)
        {
            // create new session with LoginTime = now
            var session = new WorkSession
            {
                EmpId = empId,
                LoginTime = DateTime.UtcNow
            };
            _ctx.WorkSessions.Add(session);

            // also add or update Attendance for today
            var today = DateTime.UtcNow.Date;
            var attendance = await _ctx.Attendances.FirstOrDefaultAsync(a => a.EmpId == empId && a.Date == today);
            if (attendance == null)
            {
                attendance = new Attendance
                {
                    EmpId = empId,
                    Date = today,
                    ClockIn = DateTime.UtcNow,
                    Status = "Present"
                };
                _ctx.Attendances.Add(attendance);
            }
            else
            {
                attendance.ClockIn = DateTime.UtcNow;
                attendance.Status = "Present";
                _ctx.Attendances.Update(attendance);
            }

            await _ctx.SaveChangesAsync();
            return session;
        }

        public async Task<WorkSession> ClockOutAsync(int empId)
        {
            var session = await _ctx.WorkSessions
                .Where(w => w.EmpId == empId && w.LogoutTime == null)
                .Include(w => w.Breaks)
                .OrderByDescending(w => w.LoginTime)
                .FirstOrDefaultAsync();

            if (session == null) return null;

            session.LogoutTime = DateTime.UtcNow;

            // total worked hours = difference between login and logout minus breaks
            var totalMinutes = (session.LogoutTime.Value - session.LoginTime).TotalMinutes;
            var breakMinutes = session.Breaks?.Where(b => b.BreakEndTime != null).Sum(b => b.BreakDurationMinutes) ?? 0;
            var workedMinutes = Math.Max(0, totalMinutes - breakMinutes);
            session.TotalWorkedHours = Math.Round(workedMinutes / 60.0, 2);

            // update attendance clock out
            var today = session.LoginTime.Date;
            var attendance = await _ctx.Attendances.FirstOrDefaultAsync(a => a.EmpId == empId && a.Date == today);
            if (attendance != null)
            {
                attendance.ClockOut = session.LogoutTime;
                _ctx.Attendances.Update(attendance);
            }

            _ctx.WorkSessions.Update(session);
            await _ctx.SaveChangesAsync();
            return session;
        }

        public async Task<WorkSession> GetActiveSessionAsync(int empId)
        {
            return await _ctx.WorkSessions.FirstOrDefaultAsync(w => w.EmpId == empId && w.LogoutTime == null);
        }
    }
}
