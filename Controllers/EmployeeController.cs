using EmployeeTracker.Models;
using EmployeeTracker.Repository;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class EmployeeController : ControllerBase
    {
        private readonly IGenericRepository<Employee> _repo;
        public EmployeeController(IGenericRepository<Employee> repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => Ok(await _repo.GetAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(Employee e) => Ok(await _repo.AddAsync(e));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Employee e)
        {
            var existing = await _repo.GetAsync(id);
            if (existing == null) return NotFound();
            e.Id = id;
            await _repo.UpdateAsync(e);
            return Ok(e);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
