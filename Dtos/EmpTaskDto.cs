using System;

namespace EmployeeTracker.Dtos
{
    // ------------------ DTO returned to frontend ------------------
    public class EmpTaskDto
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssigneeId { get; set; }
        public string AssigneeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }   // string for frontend
        public string Priority { get; set; } // string for frontend
        public string Tag { get; set; }
    }

    // ------------------ DTO for creating a new task ------------------
    public class CreateEmpTaskDto
    {
        public int EmpId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssigneeId { get; set; }
        public string AssigneeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Pending";   // default value
        public string Priority { get; set; } 
        public string Tag { get; set; }
    }

    // ------------------ DTO for updating an existing task ------------------
    public class UpdateEmpTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Priority { get; set; }  // string
        public string Tag { get; set; }
        public string Status { get; set; }    // string, optional
    }
}
