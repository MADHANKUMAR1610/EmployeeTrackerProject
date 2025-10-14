using AutoMapper;
using EmployeeTracker.Dtos;
using EmployeeTracker.Models;
using EmployeeTracker.Services;
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
        // Automatically approves and deducts leave balance
        [HttpPost("apply")]
        public async Task<ActionResult<LeaveRequestDto>> ApplyLeave([FromBody] CreateLeaveRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var leaveRequest = _mapper.Map<LeaveRequest>(dto);

            try
            {
                var createdLeave = await _leaveService.ApplyLeaveAsync(leaveRequest);
                var response = _mapper.Map<LeaveRequestDto>(createdLeave);
                return CreatedAtAction(nameof(GetLeavesByEmployee), new { empId = response.EmpId }, response);
            }
            catch (InvalidOperationException ex)
            {
                // Duplicate or overlapping leave error
                return BadRequest(new { message = ex.Message });
            }
        }


        // ---------------- Get Leave by Employee ----------------
        [HttpGet("byemp/{empId}")]
        public async Task<ActionResult<IEnumerable<LeaveRequestDto>>> GetLeavesByEmployee(int empId)
        {
            var leaves = await _leaveService.GetByEmpAsync(empId);
            var result = _mapper.Map<IEnumerable<LeaveRequestDto>>(leaves);
            return Ok(result);
        }

        // ---------------- Get Leave Summary for Dashboard / Attendance Page ----------------
        [HttpGet("summary/{empId}")]
        public async Task<ActionResult<IEnumerable<LeaveTypeSummaryDto>>> GetLeaveSummary(int empId)
        {
            var summary = await _leaveService.GetLeaveTypeSummaryAsync(empId);
            return Ok(summary);
        }

        // ---------------- Update Leave ----------------
        [HttpPut("update/{id}")]
        public async Task<ActionResult<LeaveRequestDto>> UpdateLeave(int id, [FromBody] CreateLeaveRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var leaveRequest = _mapper.Map<LeaveRequest>(dto);
            var updatedLeave = await _leaveService.UpdateLeaveAsync(id, leaveRequest);
            if (updatedLeave == null)
                return NotFound(new { message = $"Leave with ID {id} not found." });

            return Ok(_mapper.Map<LeaveRequestDto>(updatedLeave));
        }

        // ---------------- Delete Leave ----------------
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteLeave(int id)
        {
            var deleted = await _leaveService.DeleteLeaveAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Leave with ID {id} not found." });

            return NoContent();
        }
    }
}
