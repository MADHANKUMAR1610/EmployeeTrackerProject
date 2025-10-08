using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public Employee Employee { get; set; }

        public LeaveType LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }

        public LeaveStatus Status { get; set; } = LeaveStatus.Approved;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
