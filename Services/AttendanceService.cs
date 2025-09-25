using EmployeeTracker.Models;
using EmployeeTracker.Repository;

namespace EmployeeTracker.Services
{
    public class AttendanceService
    {
        private readonly IGenericRepository<Attendance> _repo;
            public AttendanceService(IGenericRepository<Attendance> repo) => _repo = repo;

            public Task<IEnumerable<Attendance>> GetAllAsync() => _repo.GetAllAsync();
            public Task<Attendance> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
            public Task<IEnumerable<Attendance>> GetByEmployeeAsync(int employeeId) => _repo.FindAsync(a => a.EmployeeId == employeeId);
            public Task AddAsync(Attendance a) => _repo.AddAsync(a);
            public Task UpdateAsync(Attendance a) => _repo.UpdateAsync(a);
            public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
        }
    }

