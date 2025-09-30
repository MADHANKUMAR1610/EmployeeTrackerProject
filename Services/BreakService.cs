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

        public async Task<Break> StartBreakAsync(int empId)
        {
            // ✅ Find active work session
            var sessions = await _workSessionRepo.FindAsync(w => w.EmpId == empId && w.LogoutTime == null);
            var session = sessions.FirstOrDefault();
            if (session == null) return null;

            // ✅ Create break
            var b = new Break
            {
                WorkSessionId = session.Id,
                BreakStartTime = DateTime.UtcNow
            };

            return await _breakRepo.AddAsync(b);
        }

        public async Task<Break> EndBreakAsync(int empId)
        {
            // ✅ Find active work session
            var sessions = await _workSessionRepo.FindAsync(w => w.EmpId == empId && w.LogoutTime == null);
            var session = sessions.FirstOrDefault();
            if (session == null) return null;

            // ✅ Find latest open break
            var breaks = await _breakRepo.FindAsync(b => b.WorkSessionId == session.Id && b.BreakEndTime == null);
            var br = breaks.OrderByDescending(b => b.BreakStartTime).FirstOrDefault();
            if (br == null) return null;

            // ✅ End break & calculate duration
            br.BreakEndTime = DateTime.UtcNow;
            br.BreakDurationMinutes = Math.Round(
                (br.BreakEndTime.Value - br.BreakStartTime).TotalMinutes, 2
            );

            await _breakRepo.UpdateAsync(br);
            return br;
        }
    }
}
