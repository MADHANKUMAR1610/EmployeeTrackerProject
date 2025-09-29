namespace EmployeeTracker.Dtos
{
    public class LeaveTypeSummaryDto
    {
        public string LeaveType { get; set; }
        public int TotalAllocated { get; set; }
        public int Used { get; set; }
        public int Remaining => TotalAllocated - Used;
    }
}
