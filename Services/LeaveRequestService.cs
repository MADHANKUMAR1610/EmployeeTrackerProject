using EmployeeTracker.Datas;
using EmployeeTracker.Models;
using EmployeeTracker.Repository;
using Microsoft.EntityFrameworkCore;
using System;

namespace EmployeeTracker.Services
{
    public class LeaveRequestService
    {
        private readonly EmployeeTrackerDbContext _context;
        private readonly IGenericRepository<LeaveRequest> _repo;

        public LeaveRequestService(IGenericRepository<LeaveRequest> repo, EmployeeTrackerDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public Task<IEnumerable<LeaveRequest>> GetAllAsync() => _repo.GetAllAsync();
        public Task<LeaveRequest> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<IEnumerable<LeaveRequest>> GetByEmployeeAsync(int empId) => _repo.FindAsync(l => l.EmployeeId == empId);

        public Task AddAsync(LeaveRequest lr) => _repo.AddAsync(lr);
        public Task UpdateAsync(LeaveRequest lr) => _repo.UpdateAsync(lr);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        /// <summary>
        /// SubmitLeave: create request with Pending status.
        /// We do not auto-approve here; since there are no admins, you may auto-approve by calling Approve.
        /// If you want auto-approve, change logic to call ApproveLeaveAsync.
        /// </summary>
        public async Task<LeaveRequest> SubmitLeaveAsync(LeaveRequest leave)
        {
            leave.Status = "Pending";
            leave.CreatedAt = DateTime.UtcNow;
            await _context.LeaveRequests.AddAsync(leave);
            await _context.SaveChangesAsync();
            return leave;
        }

        /// <summary>
        /// ApproveLeave: approves pending leave and deducts from LeaveBalance.
        /// If insufficient balance, returns failure message.
        /// </summary>
        public async Task<(bool Success, string Message)> ApproveLeaveAsync(int leaveId)
        {
            var leave = await _context.LeaveRequests.FindAsync(leaveId);
            if (leave == null) return (false, "Leave not found");
            if (leave.Status != "Pending") return (false, "Leave not in pending state");

            var balance = await _context.LeaveBalance.FirstOrDefaultAsync(b => b.EmployeeId == leave.EmployeeId && b.LeaveType == leave.LeaveType);
            if (balance == null) return (false, "Leave balance not found");

            var days = (int)(leave.EndTime.Date - leave.StartTime.Date).TotalDays + 1;
            if (days <= 0) return (false, "Invalid leave date range");

            if (balance.TotalLeaves - balance.UsedLeaves < days) return (false, "Insufficient leave balance");

            leave.Status = "Approved";
            balance.UsedLeaves += days;
            await _context.SaveChangesAsync();
            return (true, "Approved");
        }

        /// <summary>
        /// CancelLeave: if approved, revert used balance.
        /// </summary>
        public async Task<bool> CancelLeaveAsync(int leaveId)
        {
            var leave = await _context.LeaveRequests.FindAsync(leaveId);
            if (leave == null) return false;

            if (leave.Status == "Approved")
            {
                var balance = await _context.LeaveBalance.FirstOrDefaultAsync(b => b.EmployeeId == leave.EmployeeId && b.LeaveType == leave.LeaveType);
                if (balance != null)
                {
                    var days = (int)(leave.EndTime.Date - leave.StartTime.Date).TotalDays + 1;
                    balance.UsedLeaves = Math.Max(0, balance.UsedLeaves - days);
                }
            }

            leave.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Returns leave balances for an employee for dashboard
        /// </summary>
        public async Task<IEnumerable<LeaveBalance>> GetBalancesAsync(int employeeId)
        {
            return await _context.LeaveBalance.Where(b => b.EmployeeId == employeeId).ToListAsync();
        }
    }
}
