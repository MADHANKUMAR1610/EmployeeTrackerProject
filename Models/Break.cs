using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Models
{
    public class Break
    {
        [Key]
        public int BreakId { get; set; }
        [Required]
        public int SessionId { get; set; }
        
        public WorkSession WorkSessions { get; set; } = null!;
        [Required]
        public DateTime BreakStartTime { get; set; }
        [Required]
        public DateTime? BreakEndTime { get; set; }
    }
}
