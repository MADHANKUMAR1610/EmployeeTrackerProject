using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; }
        [Required] public string Mail { get; set; }

        // default password as Test@123
        public string Password { get; set; } = "Test@123";

        public string Role { get; set; } // e.g. Developer
        public string ProfilePictureUrl { get; set; }

        public ICollection<WorkSession> WorkSessions { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<EmpTask> TasksCreated { get; set; }
        public ICollection<EmpTask> TasksAssigned { get; set; }
        public ICollection<LeaveRequest> LeaveRequests { get; set; }
        public ICollection<LeaveBalance> LeaveBalances { get; set; }
    }
}
