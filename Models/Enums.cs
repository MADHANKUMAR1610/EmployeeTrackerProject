namespace EmployeeTracker.Models
{
    public enum LeaveType
    {

        Casual, Medical, Composition, WeekOff, Permission

    }
    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public enum TaskStatus
    {
        Pending,
        Completed
    }


    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
}
