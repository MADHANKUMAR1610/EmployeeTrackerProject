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
        private readonly IGenericRepository<WorkSession> _workSessionRepo;



        public LeaveService(
            IGenericRepository<LeaveRequest> leaveRepo,
            IGenericRepository<LeaveBalance> balanceRepo,
            IGenericRepository<WorkSession> workSessionRepo)
        {
            _leaveRepo = leaveRepo;
            _balanceRepo = balanceRepo;
              _workSessionRepo = workSessionRepo;
        }

        public async Task<LeaveRequest> ApplyLeaveAsync(LeaveRequest request)
        {
            // 1️⃣ Prevent applying overlapping leave
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

            // 2️⃣ Prevent applying leave for worked dates
            var workedDates = await _workSessionRepo.FindAsync(ws =>
                ws.EmpId == request.EmpId &&
                ws.LoginTime.Date >= request.StartDate.Date &&
                ws.LoginTime.Date <= request.EndDate.Date);

            if (workedDates.Any())
                throw new InvalidOperationException("Cannot apply leave for days you were present or already logged work sessions.");

            // 3️⃣ Auto mark as approved
            request.Status = LeaveStatus.Approved;

            // 4️⃣ Calculate total days
            var days = (int)((request.EndDate.Date - request.StartDate.Date).TotalDays) + 1;

            // 5️⃣ Get or initialize leave balance
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

            // 6️⃣ Deduct leave
            balance.UsedLeave += days;
            if (balance.UsedLeave > balance.TotalLeave)
                balance.UsedLeave = balance.TotalLeave;

            // 7️⃣ Save leave request and updated balance
            await _leaveRepo.AddAsync(request);
            await _balanceRepo.UpdateAsync(balance);
            await _leaveRepo.SaveChangesAsync();

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
