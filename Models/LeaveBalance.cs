using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeTracker.Models
{
    public class LeaveBalance
    {
        public int LeaveBalanceId { get; set; }
        public int EmployeeId { get; set; }
        
        public Employee? Employee { get; set; }
        public LeaveType LeaveType { get; set; }
        public int TotalLeaves { get; set; }
        public int UsedLeaves { get; set; }
        [NotMapped]
        public int RemainingLeaves => TotalLeaves - LeaveBalanceId;
    }
}
