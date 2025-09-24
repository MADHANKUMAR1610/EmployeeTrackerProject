namespace EmployeeTracker.Models
{
    public class WorkSession
    {
       public int SessionId { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public TimeSpan? TotalWorkedTime { get; set; }

        public ICollection<Break>? Breaks { get; set; }
    }
}

