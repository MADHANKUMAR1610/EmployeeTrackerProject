using AutoMapper;
using EmployeeTracker.Dtos;
using EmployeeTracker.Models;
using EmployeeTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _leaveService;
        private readonly IMapper _mapper;

        public LeaveController(ILeaveService leaveService, IMapper mapper)
        {
            _leaveService = leaveService;
            _mapper = mapper;
        }

        // ---------------- Apply Leave ----------------
        [HttpPost("apply")]
        public async Task<ActionResult<LeaveRequestDto>> Apply(CreateLeaveRequestDto dto)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(dto);
            var result = await _leaveService.ApplyLeaveAsync(leaveRequest);

            return Ok(_mapper.Map<LeaveRequestDto>(result));
        }

        // ---------------- Approve Leave ----------------
        [HttpPost("approve/{id}")]
        public async Task<ActionResult<LeaveRequestDto>> Approve(int id)
        {
            var result = await _leaveService.ApproveLeaveAsync(id);
            if (result == null) return NotFound();

            return Ok(_mapper.Map<LeaveRequestDto>(result));
        }

        // ---------------- Get Leave by Employee ----------------
        [HttpGet("byemp/{empId}")]
        public async Task<ActionResult<IEnumerable<LeaveRequestDto>>> ByEmp(int empId)
        {
            var leaves = await _leaveService.GetByEmpAsync(empId);
            return Ok(_mapper.Map<IEnumerable<LeaveRequestDto>>(leaves));
        }
    }
}

