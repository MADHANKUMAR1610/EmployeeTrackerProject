namespace EmployeeTracker.Dtos
{
    public class DashBoardDto
    {
        public AttandanceDto TodayAttendance { get; set; }
        public LeaveBalanceDto LeaveBalance { get; set; }
        public IEnumerable<EmpTaskDto> Tasks { get; set; } = new List<EmpTaskDto>();
    }
}
