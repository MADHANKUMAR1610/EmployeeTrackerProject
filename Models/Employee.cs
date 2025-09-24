using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        public string EmployeeName {  get; set; }
        [Required]
        public string Mail { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role{ get; set; }
        
        public string? profile_pictureUrl { get; set; }

        public ICollection<WorkSession>? WorkSessions {  get; set; }
        public ICollection<Attendance>? Attendances{ get; set; }
        public ICollection<LeaveRequest>? LeaveRequests{ get; set; }
        public ICollection<EmpTask>? Tasks { get; set; }

        
    }
}
    