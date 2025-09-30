using EmployeeTracker.Models;
using EmployeeTracker.Repository;

namespace EmployeeTracker.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<Employee> _employeeRepo;

        public AuthService(IGenericRepository<Employee> employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        public async Task<Employee> LoginAsync(string email, string password)
        {
            var employees = await _employeeRepo.FindAsync(
                e => e.Mail == email && e.Password == password
            );

            return employees.FirstOrDefault();
        }
    }
}
