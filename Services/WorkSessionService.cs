using EmployeeTracker.Datas;
using EmployeeTracker.Models;
using EmployeeTracker.Repository;
using Microsoft.EntityFrameworkCore;
using System;

namespace EmployeeTracker.Services
{
    public class WorkSessionService
    {
        private readonly EmployeeTrackerDbContext _context;
        private readonly IGenericRepository<WorkSession> _repo;

        public WorkSessionService(IGenericRepository<WorkSession> repo, EmployeeTrackerDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public Task AddAsync(WorkSession w) => _repo.AddAsync(w);
        public Task UpdateAsync(WorkSession w) => _repo.UpdateAsync(w);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
        public Task<System.Collections.Generic.IEnumerable<WorkSession>> GetAllAsync() => _repo.GetAllAsync();
        public Task<WorkSession> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<System.Collections.Generic.IEnumerable<WorkSession>> GetByEmployeeAsync(int employeeId) => _repo.FindAsync(ws => ws.EmployeeId == employeeId);

        /// <summary>
        /// Start a work session (clock-in). Creates WorkSession.LoginTime
        /// </summary>
        public async Task<WorkSession> StartSessionAsync(int employeeId)
        {
            var session = new WorkSession
            {
                EmployeeId = employeeId,
                LoginTime = DateTime.UtcNow // use UTC for consistency
            };

            await _context.WorkSessions.AddAsync(session);
            await _context.SaveChangesAsync();
            return session;
        }

        /// <summary>
        /// End session (clock-out). Calculates TotalWorkedHours = (logout-login) - breaks
        /// Break durations are taken from BreakSession entries that have BreakEndTime set.
        /// </summary>
        public async Task<WorkSession?> EndSessionAsync(int sessionId)
        {
            var session = await _context.WorkSessions
                .Include(w => w.Breaks)
                .FirstOrDefaultAsync(w => w.SessionId == sessionId);

            if (session == null || session.LogoutTime != null) return null;

            session.LogoutTime = DateTime.UtcNow;

            // total session hours (hours)
            var sessionTotalHours = (session.LogoutTime.Value - session.LoginTime).TotalHours;

            // sum up completed break durations (hours)
            var breaksHours = session.Breaks?
                .Where(b => b.BreakEndTime.HasValue)
                .Sum(b => (b.BreakEndTime!.Value - b.BreakStartTime).TotalHours) ?? 0.0;

            // compute worked hours by subtracting break durations
            session.TotalWorkedHours = Math.Max(0.0, sessionTotalHours - breaksHours);

            await _context.SaveChangesAsync();
            return session;
        }
    }
}
