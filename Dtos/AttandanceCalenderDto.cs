namespace EmployeeTracker.Dtos
{
    public class AttandanceCalenderDto
    {
        public DateTime Date { get; set; }
        public string Status { get; set; }  // Present, Absent, Leave
    }

    public class AttendanceCalendarDto
    {
        public int EmpId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public IEnumerable<AttandanceCalenderDto> Days { get; set; } = new List<AttandanceCalenderDto>();
    }
}
