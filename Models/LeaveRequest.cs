using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Models
{
    public class LeaveRequest
    {
        [Key]
        public int LeaveRequestId { get; set; }
        
        public int EmployeeId { get; set; }
        
        public Employee Employee { get; set; } = null!;
        public LeaveType LeaveType { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        
        public string Reason { get; set; } = string.Empty;
        
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
    }
}
