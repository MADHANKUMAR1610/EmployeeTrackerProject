using EmployeeTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracker.Datas
{
    public class EmployeeTrackerDbContext : DbContext
    {
        public EmployeeTrackerDbContext(DbContextOptions<EmployeeTrackerDbContext> options)
        : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<WorkSession> WorkSessions { get; set; }
        public DbSet<Break> Breaks { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<EmpTask> Tasks { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }



    }
}
