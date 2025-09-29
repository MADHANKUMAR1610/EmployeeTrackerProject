using AutoMapper;
using EmployeeTracker.Datas;
using EmployeeTracker.Dtos;
using EmployeeTracker.Models;
using EmployeeTracker.Repository;
using EmployeeTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class AttendanceController : ControllerBase
    {
        private readonly EmployeeTrackerDbContext _context;
        private readonly IMapper _mapper;

        public AttendanceController(EmployeeTrackerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("clock-in")]
        public async Task<ActionResult<AttandanceDto>> ClockIn(CreateAttendanceDto dto)
        {
            var attendance = _mapper.Map<Attendance>(dto);
            attendance.ClockIn = DateTime.Now;
            attendance.Status = "Present";

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<AttandanceDto>(attendance));
        }

        [HttpPost("clock-out/{id}")]
        public async Task<ActionResult<AttandanceDto>> ClockOut(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null) return NotFound();

            attendance.ClockOut = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<AttandanceDto>(attendance));
        }
    }
}