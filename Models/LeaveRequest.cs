using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Models
{
    public class LeaveRequest
    {
        [Key]
        public int LeaveRequestId { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public Employee Employee { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        
        public string Reason { get; set; } = string.Empty;
        
        public string Status { get; set; } = "Pending";
    }
}
