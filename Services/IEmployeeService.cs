using EmployeeTracker.Models;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetAllAsync();
    
    Task<Employee> GetByIdAsync(int id);
    Task<Employee> CreateAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task<bool> DeleteAsync(int id);

}
