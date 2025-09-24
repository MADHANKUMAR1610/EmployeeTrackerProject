using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeTracker.Models
{
    public class Break
    {
        [Key]
        public int BreakId { get; set; }
        [ForeignKey("WorkSession")]
        public int SessionId { get; set; }

        public WorkSession WorkSession { get; set; } = null!;

        [Required]
        public DateTime BreakStartTime { get; set; }
        [Required]
        public DateTime? BreakEndTime { get; set; }
    
    }
}
