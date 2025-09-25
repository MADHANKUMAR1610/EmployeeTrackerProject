using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Models
{
    public class EmpTask
    {
        [Key]
        public int TaskId { get; set; }
        
        public int EmployeeId { get; set; }
        
        public Employee Employee { get; set;} = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        
        
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Pending";

        public string Priority { get; set; } = "Medium";
    }
}
