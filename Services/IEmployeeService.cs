using EmployeeTracker.Models;

namespace EmployeeTracker.Services
{
    public interface IEmployeeService
    {

        Task<Employee> AuthenticateAsync(string email, string password);
        Task<Employee> GetByIdAsync(int id);
    }
}
