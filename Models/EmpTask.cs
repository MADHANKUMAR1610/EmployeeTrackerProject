using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Models
{
    public class EmpTask
    {
        public int Id { get; set; }

        // creator
        public int EmpId { get; set; }
        public Employee Employee { get; set; }

        // assignee (another employee reference)
        public int? AssigneeId { get; set; }
        public Employee Assignee { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.Todo;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public string Tag { get; set; }
    }
    }
