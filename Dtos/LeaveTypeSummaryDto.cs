using EmployeeTracker.Models;

namespace EmployeeTracker.Dtos
{
    public class LeaveTypeSummaryDto
    {
        public String LeaveType { get; set; }
        public int TotalAllocated { get; set; }
        public int Used { get; set; }
        public int Remaining { get; set; }
    }
}
