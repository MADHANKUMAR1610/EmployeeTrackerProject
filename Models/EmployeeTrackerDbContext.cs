using EmployeeTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracker.Datas
{
    public class EmployeeTrackerDbContext(DbContextOptions<EmployeeTrackerDbContext> options) : DbContext(options)
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<WorkSession> WorkSessions { get; set; }
        public DbSet<Break> Breaks { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<EmpTask> Tasks { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveBalance> LeaveBalance { get; set; }



    }
}
