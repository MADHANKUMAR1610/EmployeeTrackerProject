namespace EmployeeTracker.Dtos
{
        public class LeaveSummaryDto
        {
            public string LeaveType { get; set; }   // e.g., "Casual"
            public int TotalLeave { get; set; }     // e.g., 12
            public int UsedLeave { get; set; }      // e.g., 2
            public int RemainingLeave => TotalLeave - UsedLeave;
        }

    }

