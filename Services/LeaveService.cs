using EmployeeTracker.Datas;
using EmployeeTracker.Models;
using EmployeeTracker.Repository;
using Microsoft.EntityFrameworkCore;
using System;

namespace EmployeeTracker.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly EmployeeTrackerDbContext _ctx;
        public LeaveService(EmployeeTrackerDbContext ctx) => _ctx = ctx;

        public async Task<LeaveRequest> ApplyLeaveAsync(LeaveRequest request)
        {
            _ctx.LeaveRequests.Add(request);
            await _ctx.SaveChangesAsync();
            return request;
        }

        public async Task<LeaveRequest> ApproveLeaveAsync(int leaveRequestId)
        {
            var req = await _ctx.LeaveRequests.FindAsync(leaveRequestId);
            if (req == null) return null;
            if (req.Status == LeaveStatus.Approved) return req;

            // calculate days: inclusive
            var days = (int)((req.EndDate.Date - req.StartDate.Date).TotalDays) + 1;

            // find leave balance
            var balance = await _ctx.LeaveBalances.FirstOrDefaultAsync(lb => lb.EmpId == req.EmpId && lb.LeaveType == req.LeaveType);
            if (balance == null)
            {
                // create a default balance if none exists (example default: 12)
                balance = new LeaveBalance
                {
                    EmpId = req.EmpId,
                    LeaveType = req.LeaveType,
                    TotalLeave = 12,
                    UsedLeave = 0
                };
                _ctx.LeaveBalances.Add(balance);
            }

            // Deduct (simple approach) - ensure not negative
            balance.UsedLeave += days;
            if (balance.UsedLeave > balance.TotalLeave) balance.UsedLeave = balance.TotalLeave;

            req.Status = LeaveStatus.Approved;
            _ctx.LeaveRequests.Update(req);
            _ctx.LeaveBalances.Update(balance);
            await _ctx.SaveChangesAsync();
            return req;
        }

        public async Task<IEnumerable<LeaveRequest>> GetByEmpAsync(int empId)
        {
            return await Task.FromResult(_ctx.LeaveRequests.Where(r => r.EmpId == empId).AsEnumerable());
        }
    }
}
