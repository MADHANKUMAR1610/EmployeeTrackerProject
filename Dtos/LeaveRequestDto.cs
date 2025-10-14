using EmployeeTracker.Models;

namespace EmployeeTracker.Dtos
{
    public class LeaveRequestDto
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateLeaveRequestDto
    {
        public int EmpId { get; set; }
        public string LeaveType { get; set; } = string.Empty; // Casual, Medical, WeekOff, Permission
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
    }
    public class ApplyLeaveDto
    {
        public int EmpId { get; set; }
        public string LeaveType { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string? Description { get; set; }
    }
}
