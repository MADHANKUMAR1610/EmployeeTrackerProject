using EmployeeTracker.Models;
using EmployeeTracker.Repository;
using EmployeeTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IGenericRepository<EmployeeTracker.Models.Attendance> _repo;
        public AttendanceController(IGenericRepository<EmployeeTracker.Models.Attendance> repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => Ok(await _repo.GetAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeTracker.Models.Attendance a) => Ok(await _repo.AddAsync(a));
    }
}
