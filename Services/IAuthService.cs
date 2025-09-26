using EmployeeTracker.Models;

namespace EmployeeTracker.Services
{
    public interface IAuthService
    {
        Task<Employee> LoginAsync(string email, string password);
    }
}
