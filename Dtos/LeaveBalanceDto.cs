namespace EmployeeTracker.Dtos
{
    public class LeaveBalanceDto
    {

        public int Id { get; set; }
        public int EmpId { get; set; }
        public string LeaveType { get; set; }
        public int TotalLeave { get; set; }
        public int UsedLeave { get; set; }
        public int RemainingLeave { get; set; }
    }

    public class CreateLeaveBalanceDto
    {
        public int EmpId { get; set; }
        public string LeaveType { get; set; }
        public int TotalLeave { get; set; }
    }
}

