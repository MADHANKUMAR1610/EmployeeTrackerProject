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

            var session = new WorkSession
            {
                EmpId = empId,
                LoginTime = DateTime.UtcNow
            };
            _ctx.WorkSessions.Add(session);

           
            var today = DateTime.UtcNow.Date;
            var workSession = await _ctx.WorkSessions
                .FirstOrDefaultAsync(ws => ws.EmpId == empId && ws.LoginTime.Date == today);

            if (workSession == null)
            {
               
                workSession = new WorkSession
                {
                    EmpId = empId,
                    LoginTime = DateTime.UtcNow,
                    Status = "Present"
                };
                _ctx.WorkSessions.Add(workSession);
            }
            else
            {
                
                workSession.LoginTime = DateTime.UtcNow;
                workSession.Status = "Present";
                _ctx.WorkSessions.Update(workSession);
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

           
            var totalMinutes = (session.LogoutTime.Value - session.LoginTime).TotalMinutes;
            var breakMinutes = session.Breaks
            .Where(b => b.BreakEndTime != null)
            .Sum(b => (double?)b.BreakDurationMinutes) ?? 0;
            var workedMinutes = Math.Max(0, totalMinutes - breakMinutes);
            session.TotalWorkedHours = Math.Round(workedMinutes / 60.0, 2);

            
            var today = session.LoginTime.Date;
            var workSession = await _ctx.WorkSessions
                .FirstOrDefaultAsync(ws => ws.EmpId == empId && ws.LoginTime.Date == today);

            if (workSession != null)
            {
                workSession.LogoutTime = session.LogoutTime;
                workSession.TotalWorkedHours = session.TotalWorkedHours;
                _ctx.WorkSessions.Update(workSession);
            }

            await _ctx.SaveChangesAsync();
            return session;
        }


        public async Task<WorkSession> GetActiveSessionAsync(int empId)
        {
            return await _ctx.WorkSessions.FirstOrDefaultAsync(w => w.EmpId == empId && w.LogoutTime == null);
        }
    }
}
