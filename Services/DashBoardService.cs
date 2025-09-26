using AutoMapper;
using EmployeeTracker.Datas;
using EmployeeTracker.Dtos;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracker.Services
{
    public class DashBoardService : IDashboardService
    {
        private readonly EmployeeTrackerDbContext _ctx;
        private readonly IMapper _mapper;

        public DashBoardService(EmployeeTrackerDbContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<DashBoardDto> GetDashboardAsync(int empId)
        {
            var today = DateTime.UtcNow.Date;

            // Attendance
            var attendance = await _ctx.Attendances
                .FirstOrDefaultAsync(a => a.EmpId == empId && a.Date == today);

            // Leave balance
            var leaveBalance = await _ctx.LeaveBalances
                .FirstOrDefaultAsync(l => l.EmpId == empId);

            // Tasks
            var tasks = await _ctx.EmpTask
    .Where(t => t.AssigneeId == empId)
    .ToListAsync();

            return new DashBoardDto
            {
                TodayAttendance = attendance != null ? _mapper.Map<AttandanceDto>(attendance) : null,
                LeaveBalance = leaveBalance != null ? _mapper.Map<LeaveBalanceDto>(leaveBalance) : null,
                Tasks = _mapper.Map<IEnumerable<EmpTaskDto>>(tasks)
            };
        }
    }
}
