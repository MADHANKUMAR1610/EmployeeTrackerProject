using EmployeeTracker.Datas;
using EmployeeTracker.Models;
using Microsoft.EntityFrameworkCore;


namespace EmployeeTracker.Services
{
    public class LeaveBalanceService
    {
        private readonly EmployeeTrackerDbContext _context;

        public LeaveBalanceService(EmployeeTrackerDbContext context)
        {
            _context = context;
        }

        // ✅ Get all leave balances for an employee
        public async Task<IEnumerable<LeaveBalance>> GetLeaveBalances(int employeeId)
        {
            return await _context.LeaveBalance
                .Where(lb => lb.EmployeeId == employeeId)
                .ToListAsync();
        }

        // ✅ Reduce balance when leave approved
        public async Task<bool> DeductLeaveAsync(int employeeId, LeaveType leaveType, int days)
        {
            var balance = await _context.LeaveBalance
                .FirstOrDefaultAsync(lb => lb.EmployeeId == employeeId && lb.LeaveType == leaveType);

            if (balance == null) return false;

            if (balance.RemainingLeaves < days)
                return false; // ❌ Not enough balance

            balance.UsedLeaves += days;
            await _context.SaveChangesAsync();
            return true;
        }

        // ✅ Initialize default leave balance for new employee
        public async Task InitializeDefaultLeaveBalance(Employee employee)
        {
            var leaveTypes = Enum.GetValues(typeof(LeaveType)).Cast<LeaveType>();

            foreach (var type in leaveTypes)
            {
                var balance = new LeaveBalance
                {
                    EmployeeId = employee.EmployeeId,
                    LeaveType = type,
                    TotalLeaves = GetDefaultQuota(type),
                    UsedLeaves = 0
                };
                _context.LeaveBalance.Add(balance);
            }

            await _context.SaveChangesAsync();
        }

        private int GetDefaultQuota(LeaveType type)
        {
            return type switch
            {
                LeaveType.Casual => 12,
                LeaveType.Medical => 12,
                LeaveType.Composition => 2,
                LeaveType.WeekOff => 8,
                LeaveType.Permission => 3,
                _ => 0
            };
        }
    }
}