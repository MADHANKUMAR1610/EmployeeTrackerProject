using EmployeeTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracker.Datas
{
    public class EmployeeTrackerDbContext : DbContext
    {
        public EmployeeTrackerDbContext(DbContextOptions<EmployeeTrackerDbContext> opts) : base(opts) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<WorkSession> WorkSessions { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Break> Breaks { get; set; }
        public DbSet<EmpTask> EmpTask { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Task relations: created by EmpId and assigned to AssigneeId
            builder.Entity<EmpTask>()
                .HasOne(t => t.Employee)
                .WithMany(e => e.TasksCreated)
                .HasForeignKey(t => t.EmpId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<EmpTask>()
                .HasOne(t => t.Assignee)
                .WithMany(e => e.TasksAssigned)
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.Restrict);

            // 👇 Add seed data here
            builder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Test User",
                    Mail = "test@example.com",
                    Password = "Test@123",
                    Role = "Developer",
                    ProfilePictureUrl = "https://placehold.co/100x100"
                }
            );

            base.OnModelCreating(builder);
        }
    }
}

