using EmployeeTracker.Models;
using EmployeeTracker.Repository;

namespace EmployeeTracker.Services
{
    public class EmployeeService
    {
            private readonly IGenericRepository<Employee> _repo;
            public EmployeeService(IGenericRepository<Employee> repo) => _repo = repo;

            public Task<IEnumerable<Employee>> GetAllAsync() => _repo.GetAllAsync();
            public Task<Employee> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
            public async Task<Employee> GetByEmailAsync(string email)
            {
                var all = await _repo.FindAsync(e => e.Mail.ToLower() == email.ToLower());
                return all.FirstOrDefault()!;
            }
        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            // Apply default password if not already set
            if (string.IsNullOrEmpty(employee.PasswordHash?.ToString()))
            {
                // Here you can use AuthService to hash "test@123" and store it
                // For simplicity, storing raw string - but better to hash
                var defaultPassword = "test@123";
                employee.PasswordHash = System.Text.Encoding.UTF8.GetBytes(defaultPassword);
                employee.PasswordSalt = new byte[0]; // optional depending on your hashing logic
            }

            await _repo.AddAsync(employee);
            return employee;
        }


        public Task AddAsync(Employee e) => _repo.AddAsync(e);
            public Task UpdateAsync(Employee e) => _repo.UpdateAsync(e);
            public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
        }
}
