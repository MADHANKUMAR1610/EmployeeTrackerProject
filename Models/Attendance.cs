using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }
        
        public int EmployeeId { get; set; }
        
        public Employee Employee { get; set; } = null!;
        
        public DateTime Date { get; set; }
        
        public string Status { get; set; } = "Present";

    }
}
