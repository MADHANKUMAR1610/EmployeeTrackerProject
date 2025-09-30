namespace EmployeeTracker.Models
{
    public static class LeaveDefaults
    {
        public static readonly Dictionary<LeaveType, int> DefaultAllocations = new()
        {
            { LeaveType.Casual, 12 },
            { LeaveType.Medical, 12 },
            { LeaveType.WeekOff, 52 },   
            { LeaveType.Permission, 5 },
            { LeaveType.Composition, 5 }
        };

    }
}
