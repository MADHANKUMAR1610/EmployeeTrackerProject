using AutoMapper;
using EmployeeTracker.Datas;
using EmployeeTracker.Dtos;
using EmployeeTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreakController : ControllerBase
    {
        private readonly EmployeeTrackerDbContext _context;
        private readonly IMapper _mapper;

        public BreakController(EmployeeTrackerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("start/{employeeId}")]
        public async Task<ActionResult<BreakDto>> StartBreak(int employeeId)
        {
            // 1️⃣ Find the current active work session (ClockOut == null)
            var workSession = await _context.WorkSessions
                .Where(ws => ws.EmpId == employeeId && ws.LogoutTime == null)
                .OrderByDescending(ws => ws.LoginTime)
                .FirstOrDefaultAsync();

            if (workSession == null)
                return BadRequest("No active work session found for this employee.");

            // 2️⃣ Check if there's already an open break
            var existingBreak = await _context.Breaks
                .Where(b => b.WorkSessionId == workSession.Id && b.BreakEndTime == null)
                .FirstOrDefaultAsync();

            if (existingBreak != null)
                return BadRequest("Break already in progress.");

            // 3️⃣ Create a new break record
            var breakEntity = new Break
            {
                WorkSessionId = workSession.Id,
                BreakStartTime = DateTime.Now,
                BreakEndTime = null
            };

            _context.Breaks.Add(breakEntity);
            await _context.SaveChangesAsync();

            // 4️⃣ Return response (optional)
            var breakDto = new BreakDto
            {
                Id = breakEntity.Id,
                WorkSessionId = workSession.Id,
                BreakStartTime = breakEntity.BreakStartTime
            };

            return Ok(breakDto);
        }

        [HttpPost("end/{employeeId}")]
        public async Task<ActionResult> EndBreak(int employeeId)
        {
            // 1️⃣ Find the current active work session
            var workSession = await _context.WorkSessions
                .Where(ws => ws.EmpId == employeeId && ws.LogoutTime == null)
                .OrderByDescending(ws => ws.LoginTime)
                .FirstOrDefaultAsync();

            if (workSession == null)
                return BadRequest("No active work session found for this employee.");

            // 2️⃣ Find the active break
            var activeBreak = await _context.Breaks
                .Where(b => b.WorkSessionId == workSession.Id && b.BreakEndTime == null)
                .FirstOrDefaultAsync();

            if (activeBreak == null)
                return BadRequest("No active break found.");

            // 3️⃣ End the break
            activeBreak.BreakEndTime = DateTime.Now;
            activeBreak.BreakDurationMinutes =
                (int)(activeBreak.BreakEndTime.Value - activeBreak.BreakStartTime).TotalMinutes;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Break ended successfully",
                BreakDurationMinutes = activeBreak.BreakDurationMinutes
            });
        }

    }
}
