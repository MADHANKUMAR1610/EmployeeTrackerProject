using AutoMapper;
using EmployeeTracker.Datas;
using EmployeeTracker.Dtos;
using EmployeeTracker.Models;
using EmployeeTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BreakController : ControllerBase
    {
        private readonly EmployeeTrackerDbContext _context;
        private readonly IMapper _mapper;

        public BreakController(EmployeeTrackerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("start")]
        public async Task<ActionResult<BreakDto>> StartBreak(CreateBreakDto dto)
        {
            var breakEntity = _mapper.Map<Break>(dto);
            _context.Breaks.Add(breakEntity);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<BreakDto>(breakEntity));
        }

        [HttpPost("end/{id}")]
        public async Task<ActionResult<BreakDto>> EndBreak(int id)
        {
            var breakEntity = await _context.Breaks.FindAsync(id);
            if (breakEntity == null) return NotFound();

            breakEntity.BreakEndTime = DateTime.Now;
            breakEntity.BreakDurationMinutes =
                (breakEntity.BreakEndTime.Value - breakEntity.BreakStartTime).TotalMinutes;

            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<BreakDto>(breakEntity));
        }
    }
}

