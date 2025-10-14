namespace EmployeeTracker.Dtos
{
    // DTO used when sending or displaying leave balance information
    public class LeaveBalanceDto
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public string LeaveType { get; set; } = string.Empty;
        public int TotalLeave { get; set; }
        public int UsedLeave { get; set; }
        public int RemainingLeave { get; set; }
    }

    // DTO used when creating a new leave balance record
    public class CreateLeaveBalanceDto
    {
        public int EmpId { get; set; }
        public string LeaveType { get; set; } = string.Empty;
        public int TotalLeave { get; set; }
    }
}