using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public Employee Employee { get; set; } 
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Status { get; set; } = "Present";

    }
}
