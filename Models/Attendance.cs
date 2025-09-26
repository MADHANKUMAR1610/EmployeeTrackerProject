using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public Employee Employee { get; set; }

        public DateTime Date { get; set; } // date for attendance
        public DateTime? ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }
        public string Status { get; set; } // e.g., Present, Absent, OnLeave
    }
}

