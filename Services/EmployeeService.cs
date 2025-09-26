using EmployeeTracker.Datas;
using EmployeeTracker.Models;
using Microsoft.EntityFrameworkCore;


namespace EmployeeTracker.Services
{
    public class EmployeeService : IEmployeeService
    {

        private readonly EmployeeTrackerDbContext _ctx;
        public EmployeeService(EmployeeTrackerDbContext ctx) => _ctx = ctx;

        public async Task<Employee> AuthenticateAsync(string email, string password)
        {
            return await _ctx.Employees.FirstOrDefaultAsync(e => e.Mail == email && e.Password == password);
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _ctx.Employees.FindAsync(id);
        }

    }
}
