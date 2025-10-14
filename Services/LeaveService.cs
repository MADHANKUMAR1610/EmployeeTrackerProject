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
        // Automatically approves and deducts leave balance
        public async Task<LeaveRequest> ApplyLeaveAsync(LeaveRequest request)
        {
            // Check for overlapping leave for the same employee
            var existingLeaves = await _leaveRepo.FindAsync(l =>
                l.EmpId == request.EmpId &&
                (
                    (request.StartDate >= l.StartDate && request.StartDate <= l.EndDate) ||
                    (request.EndDate >= l.StartDate && request.EndDate <= l.EndDate) ||
                    (request.StartDate <= l.StartDate && request.EndDate >= l.EndDate)
                )
            );

            if (existingLeaves.Any())
                throw new InvalidOperationException("You have already applied leave for these dates.");

            // Auto mark as approved
            request.Status = LeaveStatus.Approved;

            // Calculate total days
            var days = (int)((request.EndDate.Date - request.StartDate.Date).TotalDays) + 1;

            // Get or initialize leave balance
            var balances = await _balanceRepo.FindAsync(lb =>
                lb.EmpId == request.EmpId && lb.LeaveType == request.LeaveType);

            var balance = balances.FirstOrDefault();

            if (balance == null)
            {
                balance = new LeaveBalance
                {
                    EmpId = request.EmpId,
                    LeaveType = request.LeaveType,
                    TotalLeave = request.LeaveType switch
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

            // Deduct leave
            balance.UsedLeave += days;
            if (balance.UsedLeave > balance.TotalLeave)
                balance.UsedLeave = balance.TotalLeave;

            // Save leave request and updated balance
            await _leaveRepo.AddAsync(request);
            await _balanceRepo.UpdateAsync(balance);
            await _leaveRepo.SaveChangesAsync(); // saves both because they share same DbContext

            return request;
        }

        // ---------------- Approve leave (kept for future admin flow) ----------------
        public async Task<LeaveRequest> ApproveLeaveAsync(int leaveRequestId)
        {
            var req = await _leaveRepo.GetByIdAsync(leaveRequestId);
            if (req == null) return null;
            if (req.Status == LeaveStatus.Approved) return req;

            var days = (int)((req.EndDate.Date - req.StartDate.Date).TotalDays) + 1;

            var balances = await _balanceRepo.FindAsync(lb =>
                lb.EmpId == req.EmpId && lb.LeaveType == req.LeaveType);

            var balance = balances.FirstOrDefault();

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

            balance.UsedLeave += days;
            if (balance.UsedLeave > balance.TotalLeave)
                balance.UsedLeave = balance.TotalLeave;

            req.Status = LeaveStatus.Approved;

            await _leaveRepo.UpdateAsync(req);
            await _balanceRepo.UpdateAsync(balance);
            await _leaveRepo.SaveChangesAsync();

            return req;
        }

        // ---------------- Get leaves by employee ----------------
        public async Task<IEnumerable<LeaveRequest>> GetByEmpAsync(int empId)
        {
            return await _leaveRepo.FindAsync(r => r.EmpId == empId);
        }

        // ---------------- Get leave summary ----------------
        public async Task<IEnumerable<LeaveTypeSummaryDto>> GetLeaveTypeSummaryAsync(int empId)
        {
            var balances = await _balanceRepo.FindAsync(lb => lb.EmpId == empId);

            return balances.Select(lb => new LeaveTypeSummaryDto
            {
                LeaveType = lb.LeaveType.ToString(),
                TotalAllocated = lb.TotalLeave,
                Used = lb.UsedLeave,
                Remaining = lb.TotalLeave - lb.UsedLeave // ✅ Explicitly include Remaining
            });
        }

        // ---------------- Update leave ----------------
        public async Task<LeaveRequest> UpdateLeaveAsync(int id, LeaveRequest updatedRequest)
        {
            var existing = await _leaveRepo.GetByIdAsync(id);
            if (existing == null) return null;

            existing.StartDate = updatedRequest.StartDate;
            existing.EndDate = updatedRequest.EndDate;
            existing.LeaveType = updatedRequest.LeaveType;
            existing.Reason = updatedRequest.Reason;
            existing.Status = updatedRequest.Status;

            await _leaveRepo.UpdateAsync(existing);
            await _leaveRepo.SaveChangesAsync();

            return existing;
        }

        // ---------------- Delete leave ----------------
        public async Task<bool> DeleteLeaveAsync(int id)
        {
            var leave = await _leaveRepo.GetByIdAsync(id);
            if (leave == null) return false;

            await _leaveRepo.DeleteAsync(id);
            await _leaveRepo.SaveChangesAsync();
            return true;
        }
    }
}
