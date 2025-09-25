using EmployeeTracker.Datas;
using EmployeeTracker.Models;
using EmployeeTracker.Repository;
using Microsoft.EntityFrameworkCore;
using System;

namespace EmployeeTracker.Services
{
    public class BreakService
    {
        private readonly EmployeeTrackerDbContext _context;
        private readonly IGenericRepository<Break> _repo;

        public BreakService(IGenericRepository<Break> repo, EmployeeTrackerDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public Task AddAsync(Break b) => _repo.AddAsync(b);
        public Task UpdateAsync(Break b) => _repo.UpdateAsync(b);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
        public Task<System.Collections.Generic.IEnumerable<Break>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Break> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        /// <summary>
        /// StartBreak: create break record with BreakStartTime.
        /// </summary>
        public async Task<Break> StartBreakAsync(int sessionId)
        {
            var brk = new Break
            {
                SessionId = sessionId,
                BreakStartTime = DateTime.UtcNow,

            };

            await _context.Breaks.AddAsync(brk);
            await _context.SaveChangesAsync();
            return brk;
        }

        /// <summary>
        /// EndBreak: sets BreakEndTime and BreakDurationHours.
        /// BreakDurationHours will be used to subtract from total worked hours when session ends.
        /// </summary>
        public async Task<Break?> EndBreakAsync(int breakId)
        {
            var brk = await _context.Breaks.FirstOrDefaultAsync(b => b.BreakId == breakId);
            if (brk == null || brk.BreakEndTime.HasValue) return null;

            brk.BreakEndTime = DateTime.UtcNow;
            brk.BreakDurationHours = (brk.BreakEndTime.Value - brk.BreakStartTime).TotalHours;
            await _context.SaveChangesAsync();
            return brk;
        }
    }
}
