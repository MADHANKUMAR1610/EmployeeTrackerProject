using EmployeeTracker.Models;
using EmployeeTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {

        private readonly EmployeeService _service;
        public EmployeeController(EmployeeService service) => _service = service;

        // Get all employees
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        // Get by id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var emp = await _service.GetByIdAsync(id);
            if (emp == null) return NotFound();
            return Ok(emp);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            var created = await _service.CreateEmployeeAsync(employee);
            return CreatedAtAction(nameof(Get), new { id = created.EmployeeId }, created);
        }


        // Update profile (employee may update their own details)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Employee e)
        {
            if (id != e.EmployeeId) return BadRequest();
            await _service.UpdateAsync(e);
            return NoContent();
        }

        // Delete (if necessary)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
