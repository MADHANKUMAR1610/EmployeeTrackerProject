using EmployeeTracker.Datas;
using EmployeeTracker.Dtos;
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

        // Apply leave
        public async Task<LeaveRequest> ApplyLeaveAsync(LeaveRequest request)
        {
            _ctx.LeaveRequests.Add(request);
            await _ctx.SaveChangesAsync();
            return request;
        }

        // Approve leave
        public async Task<LeaveRequest> ApproveLeaveAsync(int leaveRequestId)
        {
            var req = await _ctx.LeaveRequests.FindAsync(leaveRequestId);
            if (req == null) return null;
            if (req.Status == LeaveStatus.Approved) return req;

            var days = (int)((req.EndDate.Date - req.StartDate.Date).TotalDays) + 1;

            var balance = await _ctx.LeaveBalances
                .FirstOrDefaultAsync(lb => lb.EmpId == req.EmpId && lb.LeaveType == req.LeaveType);

            if (balance == null)
            {
                balance = new LeaveBalance
                {
                    EmpId = req.EmpId,
                    LeaveType = req.LeaveType,
                    TotalLeave = 12,
                    UsedLeave = 0
                };
                _ctx.LeaveBalances.Add(balance);
            }

            balance.UsedLeave += days;
            if (balance.UsedLeave > balance.TotalLeave) balance.UsedLeave = balance.TotalLeave;

            req.Status = LeaveStatus.Approved;
            _ctx.LeaveRequests.Update(req);

            await _ctx.SaveChangesAsync();
            return req;
        }

        // Get leaves by employee
        public async Task<IEnumerable<LeaveRequest>> GetByEmpAsync(int empId)
        {
            return await _ctx.LeaveRequests
                .Where(r => r.EmpId == empId)
                .ToListAsync();
        }

        // 🔹 Get leave summary (for attendance page)
        public async Task<IEnumerable<LeaveSummaryDto>> GetLeaveSummaryAsync(int empId)
        {
            var balances = await _ctx.LeaveBalances
                .Where(lb => lb.EmpId == empId)
                .ToListAsync();

            return balances.Select(lb => new LeaveSummaryDto
            {
                LeaveType = lb.LeaveType.ToString(),
                TotalLeave = lb.TotalLeave,
                UsedLeave = lb.UsedLeave,

            });
        }
        public async Task<IEnumerable<LeaveTypeSummaryDto>> GetLeaveTypeSummaryAsync(int empId)
        {
            var balances = await _ctx.LeaveBalances
                .Where(lb => lb.EmpId == empId)
                .Select(lb => new LeaveTypeSummaryDto
                {
                    LeaveType = lb.LeaveType.ToString(),
                    TotalAllocated = lb.TotalLeave,
                    Used = lb.UsedLeave
                })
                .ToListAsync();

            return balances;
        }
        public async Task<int> GetPendingLeaveCountAsync(int empId)
        {
            return await _ctx.LeaveRequests
                .CountAsync(l => l.EmpId == empId && l.Status == Models.LeaveStatus.Pending);
        }

    }
}