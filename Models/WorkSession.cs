using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Models
{
    public class WorkSession
    {
        [Key]
       public int SessionId { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public DateTime LoginTime { get; set; }

        [Required]
        public DateTime? LogoutTime { get; set; }
        [Required]
        public TimeSpan? TotalWorkedTime { get; set; }

        public ICollection<Break>? Breaks { get; set; }
    }
}

