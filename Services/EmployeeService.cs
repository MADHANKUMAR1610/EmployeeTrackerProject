using EmployeeTracker.Models;
using EmployeeTracker.Repository;

namespace EmployeeTracker.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IGenericRepository<Employee> _employeeRepo;

        public EmployeeService(IGenericRepository<Employee> employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        // ---------------- Authenticate ----------------
        public async Task<Employee> AuthenticateAsync(string email, string password)
        {
            var employees = await _employeeRepo.FindAsync(e => e.Mail == email && e.Password == password);
            return employees.FirstOrDefault();
        }

        // ---------------- Get employee by Id ----------------
        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _employeeRepo.GetByIdAsync(id);
        }
    }
}


