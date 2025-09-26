namespace EmployeeTracker.Dtos
{
    public class EmpTaskDto
    {

        public int Id { get; set; }
        public int EmpId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssigneeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Tag { get; set; }
    }

    public class CreateEmpTaskDto
    {
        public int EmpId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssigneeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Tag { get; set; }
    }
}
