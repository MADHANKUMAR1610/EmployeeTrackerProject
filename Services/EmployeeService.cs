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

        // ---------------- Get employee by Id ----------------
        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _employeeRepo.GetByIdAsync(id);
        }
        // ---------------- Get by Id ----------------
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employeeRepo.GetAllAsync();
        }


        // ---------------- Create ----------------
        public async Task<Employee> CreateAsync(Employee employee)
        {
            // Default password rule
            if (string.IsNullOrEmpty(employee.Password))
                employee.Password = "Test@123";

            await _employeeRepo.AddAsync(employee);
            await _employeeRepo.SaveChangesAsync();
            return employee;
        }

        // ---------------- Update ----------------
        public async Task<Employee> UpdateAsync(Employee employee)
        {
            await _employeeRepo.UpdateAsync(employee);
            await _employeeRepo.SaveChangesAsync();
            return employee;
        }

        // ---------------- Delete ----------------
        public async Task<bool> DeleteAsync(int id)
        {
            var emp = await _employeeRepo.GetByIdAsync(id);
            if (emp == null) return false;

            await _employeeRepo.DeleteAsync(id);
            await _employeeRepo.SaveChangesAsync();
            return true;
        }
    }
}


