using EmployeeTracker.Datas;
using EmployeeTracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeTracker.Services
{
    public class AuthServices
    {

        private readonly EmployeeTrackerDbContext _context;
        public AuthServices(EmployeeTrackerDbContext context) => _context = context;

        // Register - create an employee with hashed default or provided password
        public async Task<Employee?> RegisterAsync(string name, string mail, string password = "test@123")
        {
            if (await _context.Employees.AnyAsync(e => e.Mail == mail)) return null;

            using var hmac = new HMACSHA512();
            var emp = new Employee
            {
                EmployeeName = name,
                Mail = mail,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };

            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();

            // seed default leave balances for employee (policy: change numbers as needed)
            SeedDefaultLeaveBalances(emp.EmployeeId);

            return emp;
        }

        // Check credentials
        public async Task<Employee?> AuthenticateAsync(string mail, string password)
        {
            var emp = await _context.Employees.FirstOrDefaultAsync(e => e.Mail == mail);
            if (emp == null) return null;

            using var hmac = new HMACSHA512(emp.PasswordSalt!);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            if (!computed.SequenceEqual(emp.PasswordHash!)) return null;
            return emp;
        }

        private void SeedDefaultLeaveBalances(int employeeId)
        {
            var defaults = new List<LeaveBalance>
            {
                new LeaveBalance { EmployeeId = employeeId, LeaveType = LeaveType.Casual, TotalLeaves = 12, UsedLeaves = 0 },
                new LeaveBalance { EmployeeId = employeeId, LeaveType = LeaveType.Medical, TotalLeaves = 10, UsedLeaves = 0 },
                new LeaveBalance { EmployeeId = employeeId, LeaveType = LeaveType.Composition, TotalLeaves = 5, UsedLeaves = 0 },
                new LeaveBalance { EmployeeId = employeeId, LeaveType = LeaveType.WeekOff, TotalLeaves = 12, UsedLeaves = 0 },
                new LeaveBalance { EmployeeId = employeeId, LeaveType = LeaveType.Permission, TotalLeaves = 6, UsedLeaves = 0 }
            };

            _context.LeaveBalance.AddRange(defaults);
            _context.SaveChanges();
        }
    }
}
