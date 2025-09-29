using AutoMapper;
using EmployeeTracker.Datas;
using EmployeeTracker.Dtos;
using EmployeeTracker.Models;
using EmployeeTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class TaskController : ControllerBase
    {
        private readonly EmployeeTrackerDbContext _context;
        private readonly IMapper _mapper;

        public TaskController(EmployeeTrackerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<EmpTaskDto>> CreateTask(CreateEmpTaskDto dto)
        {
            var task = _mapper.Map<EmpTask>(dto);
            _context.EmpTask.Add(task);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<EmpTaskDto>(task));
        }

        [HttpGet("by-employee/{empId}")]
        public async Task<ActionResult<IEnumerable<EmpTaskDto>>> GetTasksByEmployee(int empId)
        {
            var tasks = await _context.EmpTask
                .Where(t => t.EmpId == empId || t.AssigneeId == empId)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<EmpTaskDto>>(tasks));
        }
    }
}
