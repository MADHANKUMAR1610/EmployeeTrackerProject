namespace EmployeeTracker.Dtos
{
    public class BreakDto
    {
        public int Id { get; set; }
        public int WorkSessionId { get; set; }
        public DateTime BreakStartTime { get; set; }
        public DateTime? BreakEndTime { get; set; }
        public double BreakDurationMinutes { get; set; }
    }

    public class CreateBreakDto
    {
        public int WorkSessionId { get; set; }
            public DateTime BreakStartTime { get; set; }

            // Add any other necessary fields
        

    }
}
