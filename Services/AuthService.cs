using EmployeeTracker.Datas;
using EmployeeTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeTracker.Services
{
    public class AuthService
    {
        private readonly EmployeeTrackerDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(EmployeeTrackerDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Login with email and default password (test@123)
        public async Task<string?> LoginAsync(string email, string password)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Mail == email);
            if (employee == null) return null;

            if (!VerifyPassword(password, employee.PasswordHash!, employee.PasswordSalt!))
                return null;

            return GenerateJwtToken(employee);
        }

        // Set default password for new employees
        public void SetDefaultPassword(Employee employee)
        {
            CreatePasswordHash("test@123", out byte[] hash, out byte[] salt);
            employee.PasswordHash = hash;
            employee.PasswordSalt = salt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }

        private string GenerateJwtToken(Employee employee)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, employee.EmployeeId.ToString()),
            new Claim(ClaimTypes.Email, employee.Mail),
            new Claim(ClaimTypes.Role, "Employee")
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
