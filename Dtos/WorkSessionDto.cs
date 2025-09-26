namespace EmployeeTracker.Dtos
{
    public class WorkSessionDto
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public double TotalWorkedHours { get; set; }
    }

    public class CreateWorkSessionDto
    {
        public int EmpId { get; set; }
        public DateTime LoginTime { get; set; } = DateTime.Now;
    }

}
