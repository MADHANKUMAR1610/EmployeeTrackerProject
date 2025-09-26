using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeTracker.Models
{
    public class LeaveBalance
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public Employee Employee { get; set; }

        public LeaveType LeaveType { get; set; }
        public int TotalLeave { get; set; }
        public int UsedLeave { get; set; }

        [NotMapped]
        public int RemainingLeave => TotalLeave - UsedLeave;
    }
}
