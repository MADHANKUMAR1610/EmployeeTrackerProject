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

        public Task AddAsync(Employee e) => _repo.AddAsync(e);
            public Task UpdateAsync(Employee e) => _repo.UpdateAsync(e);
            public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
        }
}
