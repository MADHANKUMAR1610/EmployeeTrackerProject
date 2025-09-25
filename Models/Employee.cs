using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        
        public string EmployeeName { get; set; } = null!;
        
        public string Mail { get; set; } = null!;
        
        public string Password { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string? profile_pictureUrl { get; set; }

        public ICollection<WorkSession>? WorkSessions {  get; set; }
        public ICollection<Attendance>? Attendances{ get; set; }
        public ICollection<LeaveRequest>? LeaveRequests{ get; set; }
        public ICollection<EmpTask>? Tasks { get; set; }

        
    }
}
    