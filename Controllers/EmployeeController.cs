using AutoMapper;
using EmployeeTracker.Dtos;
using EmployeeTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        // ---------------- GET all ----------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var employees = await _employeeService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(employees));
        }

        // ---------------- GET by id ----------------
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null) return NotFound();

            return Ok(_mapper.Map<EmployeeDto>(employee));
        }

        // ---------------- POST ----------------
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee(CreateEmployeeDto dto)
        {
            var employee = _mapper.Map<Employee>(dto);
            var created = await _employeeService.CreateAsync(employee);

            return Ok(_mapper.Map<EmployeeDto>(created));
        }

        // ---------------- PUT ----------------
        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployee(int id, UpdateEmployeeDto dto)
        {
            var existing = await _employeeService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            _mapper.Map(dto, existing);
            var updated = await _employeeService.UpdateAsync(existing);

            return Ok(_mapper.Map<EmployeeDto>(updated));
        }

        // ---------------- DELETE ----------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var deleted = await _employeeService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}

