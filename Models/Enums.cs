namespace EmployeeTracker.Models
{
    public enum LeaveType
    {

        Casual = 12, Medical = 12, Composition = 8, WeekOff = 8, Permission = 3

    }
    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public enum TaskStatus
    {
        Todo,
        InProgress,
        Done,
        Blocked
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
}
