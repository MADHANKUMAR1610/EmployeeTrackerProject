namespace EmployeeTracker.Dtos
{
    public class AttandanceDto
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public DateTime Date { get; set; }
        public DateTime ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }
        public string Status { get; set; }
    }

    public class CreateAttendanceDto
    {
        public int EmpId { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
    }
}
