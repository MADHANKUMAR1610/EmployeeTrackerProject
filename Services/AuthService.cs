using EmployeeTracker.Datas;
using EmployeeTracker.Models;
using Microsoft.EntityFrameworkCore;


namespace EmployeeTracker.Services
{
    public class AuthService : IAuthService
    {
        private readonly EmployeeTrackerDbContext _context;

        public AuthService(EmployeeTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> LoginAsync(string email, string password)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(x => x.Mail == email && x.Password == password);
        }
    }
}
