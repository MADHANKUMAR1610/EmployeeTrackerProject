using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Models
{
    public class WorkSession
    {
        [Key]
       public int SessionId { get; set; }
        
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public DateTime LoginTime { get; set; }

        
        public DateTime? LogoutTime { get; set; }
        
        public double TotalWorkedHours { get; set; }

        public ICollection<Break>? Breaks { get; set; }
    }
}

