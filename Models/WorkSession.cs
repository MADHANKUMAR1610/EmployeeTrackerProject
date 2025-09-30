using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Models
{
    public class WorkSession
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public Employee Employee { get; set; }

        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
         public string Status { get; set; } 

        // stored as hours (double). Calculated on logout.
        public double TotalWorkedHours { get; set; }

        public ICollection<Break> Breaks { get; set; }
    }
}

