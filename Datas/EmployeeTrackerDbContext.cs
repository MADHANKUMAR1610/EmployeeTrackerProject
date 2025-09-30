using EmployeeTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracker.Datas
{
    public class EmployeeTrackerDbContext : DbContext
    {
        public EmployeeTrackerDbContext(DbContextOptions<EmployeeTrackerDbContext> opts) : base(opts) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<WorkSession> WorkSessions { get; set; }
        
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
            builder.Entity<LeaveBalance>().HasData(
            new LeaveBalance { Id = 1, EmpId = 1, LeaveType = LeaveType.Casual, TotalLeave = 12, UsedLeave = 0 },
            new LeaveBalance { Id = 2, EmpId = 1, LeaveType = LeaveType.Medical, TotalLeave = 12, UsedLeave = 0 },
            new LeaveBalance { Id = 3, EmpId = 1, LeaveType = LeaveType.Permission, TotalLeave = 5, UsedLeave = 0 },
            new LeaveBalance { Id = 4, EmpId = 1, LeaveType = LeaveType.WeekOff, TotalLeave = 52, UsedLeave = 0 },
                
            new LeaveBalance { Id = 5, EmpId = 1, LeaveType = LeaveType.Composition, TotalLeave = 52, UsedLeave = 0 }
        );


            base.OnModelCreating(builder);
        }
    }
}

