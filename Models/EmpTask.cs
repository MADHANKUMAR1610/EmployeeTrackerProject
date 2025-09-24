using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Models
{
    public class EmpTask
    {
        [Key]
        public int TaskId { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public Employee Employee { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Pending";

        public string Priority { get; set; } = "Medium";
    }
}
