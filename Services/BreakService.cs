using EmployeeTracker.Models;
using EmployeeTracker.Repository;

namespace EmployeeTracker.Services
{
    public class BreakService : IBreakService
    {
        private readonly IGenericRepository<WorkSession> _workSessionRepo;
        private readonly IGenericRepository<Break> _breakRepo;

        public BreakService(
            IGenericRepository<WorkSession> workSessionRepo,
            IGenericRepository<Break> breakRepo)
        {
            _workSessionRepo = workSessionRepo;
            _breakRepo = breakRepo;
        }   

        // ✅ Start a new break
        public async Task<Break> StartBreakAsync(int empId)
        {
            // Find active work session
            var sessions = await _workSessionRepo.FindAsync(w => w.EmpId == empId && w.LogoutTime == null);
            var session = sessions.FirstOrDefault();
            if (session == null)
                return null;

            // Create new break entry
            var newBreak = new Break
            {
                WorkSessionId = session.Id,
                BreakStartTime = DateTime.Now, // use local time for readability
                BreakEndTime = null,
                BreakDurationMinutes = 0
            };

            return await _breakRepo.AddAsync(newBreak);
        }

        // ✅ End the latest active break
        public async Task<Break> EndBreakAsync(int empId)
        {
            // Find active work session
            var sessions = await _workSessionRepo.FindAsync(w => w.EmpId == empId && w.LogoutTime == null);
            var session = sessions.FirstOrDefault();
            if (session == null)
                return null;

            // Find the last open break
            var breaks = await _breakRepo.FindAsync(b => b.WorkSessionId == session.Id && b.BreakEndTime == null);
            var activeBreak = breaks.OrderByDescending(b => b.BreakStartTime).FirstOrDefault();
            if (activeBreak == null)
                return null;

            // End break and calculate duration
            activeBreak.BreakEndTime = DateTime.Now;
            var duration = activeBreak.BreakEndTime.Value - activeBreak.BreakStartTime;

            // Store clean duration in minutes
            activeBreak.BreakDurationMinutes = Math.Round(duration.TotalMinutes, 2);

            await _breakRepo.UpdateAsync(activeBreak);
            return activeBreak;
        }
    }
}