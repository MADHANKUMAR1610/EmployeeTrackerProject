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

        // ✅ Start Break
        [HttpPost("start")]
        public async Task<ActionResult<BreakDto>> StartBreak(CreateBreakDto dto)
        {
            // Validate WorkSession existence
            bool workSessionExists = await _context.WorkSessions
                .AnyAsync(ws => ws.Id == dto.WorkSessionId);

            if (!workSessionExists)
            {
                return BadRequest("Invalid WorkSessionId. The referenced WorkSession does not exist.");
            }

            // Create new break entry
            var breakEntity = _mapper.Map<Break>(dto);
            breakEntity.BreakStartTime = DateTime.Now;
            breakEntity.BreakEndTime = null;
            breakEntity.BreakDurationMinutes = 0;

            _context.Breaks.Add(breakEntity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<BreakDto>(breakEntity);
            return Ok(result);
        }

        // ✅ End Break
        [HttpPost("end/{id}")]
        public async Task<ActionResult<BreakDto>> EndBreak(int id)
        {
            var breakEntity = await _context.Breaks.FindAsync(id);
            if (breakEntity == null)
                return NotFound("Break not found.");

            if (breakEntity.BreakEndTime != null)
                return BadRequest("Break has already been ended.");

            breakEntity.BreakEndTime = DateTime.Now;

            // Calculate accurate duration in minutes
            var duration = breakEntity.BreakEndTime.Value - breakEntity.BreakStartTime;
            breakEntity.BreakDurationMinutes = Math.Round(duration.TotalMinutes, 2); // 👈 Round to 2 decimals

            await _context.SaveChangesAsync();

            var result = _mapper.Map<BreakDto>(breakEntity);
            return Ok(result);
        }
    }
}