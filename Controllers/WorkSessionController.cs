using AutoMapper;
using EmployeeTracker.Datas;
using EmployeeTracker.Dtos;
using EmployeeTracker.Models;
using EmployeeTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkSessionController : ControllerBase
    {
        private readonly EmployeeTrackerDbContext _context;
        private readonly IMapper _mapper;

        public WorkSessionController(EmployeeTrackerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<WorkSessionDto>> StartSession(CreateWorkSessionDto dto)
        {
            var session = _mapper.Map<WorkSession>(dto);
            _context.WorkSessions.Add(session);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<WorkSessionDto>(session));
        }

        [HttpPost("logout/{id}")]
        public async Task<ActionResult<WorkSessionDto>> EndSession(int id)
        {
            var session = await _context.WorkSessions.FindAsync(id);
            if (session == null) return NotFound();

            session.LogoutTime = DateTime.Now;
            session.TotalWorkedHours = (session.LogoutTime.Value - session.LoginTime).TotalHours;

            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<WorkSessionDto>(session));
        }
    }
}
