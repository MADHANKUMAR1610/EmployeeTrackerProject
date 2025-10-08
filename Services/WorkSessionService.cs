using EmployeeTracker.Models;
using EmployeeTracker.Repository;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracker.Services
{
    public class WorkSessionService : IWorkSessionService
    {
        private readonly IGenericRepository<WorkSession> _workSessionRepo;

        public WorkSessionService(IGenericRepository<WorkSession> workSessionRepo)
        {
            _workSessionRepo = workSessionRepo;
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
            
            var newSession = await _workSessionRepo.AddAsync(session);
            await _workSessionRepo.SaveChangesAsync();
            return newSession;
        }

        // Clock Out (end work session)
        public async Task<WorkSession> ClockOutAsync(int sessionId)
        {
            var session = await _workSessionRepo.GetByIdAsync(sessionId);
            if (session == null) return null;

            session.LogoutTime = DateTime.Now;

            if (session.LogoutTime.HasValue)
                session.TotalWorkedHours =
                    (session.LogoutTime.Value - session.LoginTime).TotalHours;

            _workSessionRepo.Update(session);
            await _workSessionRepo.SaveChangesAsync();
            return session;
        }

        // Get currently active session (no logout yet)
        public async Task<WorkSession> GetActiveSessionAsync(int empId)
        {
            var sessions = await _workSessionRepo.FindAsync(
                ws => ws.EmpId == empId && ws.LogoutTime == null
            );

            return sessions
                .OrderByDescending(ws => ws.LoginTime)
                .FirstOrDefault();
        }
    }
}
