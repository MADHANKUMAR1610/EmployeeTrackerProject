using EmployeeTracker.Datas;
using EmployeeTracker.Models;
using EmployeeTracker.Repository;
using Microsoft.EntityFrameworkCore;
using System;

namespace EmployeeTracker.Services
{
    public class BreakService : IBreakService
    {
        private readonly EmployeeTrackerDbContext _ctx;
        public BreakService(EmployeeTrackerDbContext ctx) => _ctx = ctx;

        public async Task<Break> StartBreakAsync(int empId)
        {
            // get active worksession
            var session = await _ctx.WorkSessions.FirstOrDefaultAsync(w => w.EmpId == empId && w.LogoutTime == null);
            if (session == null) return null;

            var b = new Break
            {
                WorkSessionId = session.Id,
                BreakStartTime = DateTime.UtcNow
            };
            _ctx.Breaks.Add(b);
            await _ctx.SaveChangesAsync();
            return b;
        }

        public async Task<Break> EndBreakAsync(int empId)
        {
            // find latest open break for active session
            var session = await _ctx.WorkSessions.FirstOrDefaultAsync(w => w.EmpId == empId && w.LogoutTime == null);
            if (session == null) return null;

            var br = await _ctx.Breaks
                .Where(b => b.WorkSessionId == session.Id && b.BreakEndTime == null)
                .OrderByDescending(b => b.BreakStartTime)
                .FirstOrDefaultAsync();

            if (br == null) return null;

            br.BreakEndTime = DateTime.UtcNow;
            var minutes = (br.BreakEndTime.Value - br.BreakStartTime).TotalMinutes;
            br.BreakDurationMinutes = Math.Round((br.BreakEndTime.Value - br.BreakStartTime).TotalMinutes, 2);


            _ctx.Breaks.Update(br);
            await _ctx.SaveChangesAsync();
            return br;
        }
    }
}
