using EmployeeTracker.Dtos;
using EmployeeTracker.Models;
using EmployeeTracker.Repository;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracker.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly IGenericRepository<LeaveRequest> _leaveRepo;
        private readonly IGenericRepository<LeaveBalance> _balanceRepo;

        public LeaveService(
            IGenericRepository<LeaveRequest> leaveRepo,
            IGenericRepository<LeaveBalance> balanceRepo)
        {
            _leaveRepo = leaveRepo;
            _balanceRepo = balanceRepo;
        }

        // ---------------- Apply leave ----------------
        public async Task<LeaveRequest> ApplyLeaveAsync(LeaveRequest request)
        {
            await _leaveRepo.AddAsync(request);
            await _leaveRepo.SaveChangesAsync();
            return request;
        }

        // ---------------- Approve leave ----------------
        public async Task<LeaveRequest> ApproveLeaveAsync(int leaveRequestId)
        {
            var req = await _leaveRepo.GetByIdAsync(leaveRequestId);
            if (req == null) return null;
            if (req.Status == LeaveStatus.Approved) return req;

            var days = (int)((req.EndDate.Date - req.StartDate.Date).TotalDays) + 1;

            var balances = await _balanceRepo.FindAsync(lb =>
                lb.EmpId == req.EmpId && lb.LeaveType == req.LeaveType);

            var balance = balances.FirstOrDefault();

            // ✅ Initialize leave balance if not present
            if (balance == null)
            {
                balance = new LeaveBalance
                {
                    EmpId = req.EmpId,
                    LeaveType = req.LeaveType,
                    TotalLeave = req.LeaveType switch
                    {
                        LeaveType.Casual => 12,
                        LeaveType.Medical => 12,
                        LeaveType.Permission => 5,
                        LeaveType.WeekOff => 52,
                        LeaveType.Composition => 5,
                        _ => 0
                    },
                    UsedLeave = 0
                };
                await _balanceRepo.AddAsync(balance);
            }

            // ✅ Deduct from correct leave type
            balance.UsedLeave += days;
            if (balance.UsedLeave > balance.TotalLeave)
                balance.UsedLeave = balance.TotalLeave;

            req.Status = LeaveStatus.Approved;
            await _leaveRepo.UpdateAsync(req);      
await _balanceRepo.UpdateAsync(balance); 

            await _leaveRepo.SaveChangesAsync();  // save changes for both repos (same DbContext underneath)
            return req;
        }

        // ---------------- Get leaves by employee ----------------
        public async Task<IEnumerable<LeaveRequest>> GetByEmpAsync(int empId)
        {
            return await _leaveRepo.FindAsync(r => r.EmpId == empId);
        }

        // ---------------- Get leave summary (for dashboard display) ----------------
        public async Task<IEnumerable<LeaveTypeSummaryDto>> GetLeaveTypeSummaryAsync(int empId)
        {
            var balances = await _balanceRepo.FindAsync(lb => lb.EmpId == empId);

            return balances.Select(lb => new LeaveTypeSummaryDto
            {
                LeaveType = lb.LeaveType.ToString(),
                TotalAllocated = lb.TotalLeave,
                Used = lb.UsedLeave
            });
        }
    }
}
