using EmployeeTracker.Datas;
using EmployeeTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracker.Services
{
    public class WorkSessionService : IWorkSessionService
    {
        private readonly EmployeeTrackerDbContext _ctx;

        public WorkSessionService(EmployeeTrackerDbContext ctx)
        {
            _ctx = ctx;
        }

        // Clock In (start work session)
        public async Task<WorkSession> ClockInAsync(int empId)
        {
            var session = new WorkSession
            {
                EmpId = empId,
                LoginTime = DateTime.Now,
                Breaks = new List<Break>()
            };

            _ctx.WorkSessions.Add(session);
            await _ctx.SaveChangesAsync();
            return session;
        }

        // Clock Out (end work session)
        public async Task<WorkSession> ClockOutAsync(int sessionId)
        {
            var session = await _ctx.WorkSessions.FindAsync(sessionId);
            if (session == null) return null;

            session.LogoutTime = DateTime.Now;

            if (session.LogoutTime.HasValue)
                session.TotalWorkedHours =
                    (session.LogoutTime.Value - session.LoginTime).TotalHours;

            await _ctx.SaveChangesAsync();
            return session;
        }

        // Get currently active session (no logout yet)
        public async Task<WorkSession> GetActiveSessionAsync(int empId)
        {
            return await _ctx.WorkSessions
                .Where(ws => ws.EmpId == empId && ws.LogoutTime == null)
                .OrderByDescending(ws => ws.LoginTime)
                .FirstOrDefaultAsync();
        }
    }
}
