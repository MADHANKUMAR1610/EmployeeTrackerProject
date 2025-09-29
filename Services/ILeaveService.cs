using EmployeeTracker.Dtos;
using EmployeeTracker.Models;

namespace EmployeeTracker.Services
{
    public interface ILeaveService
    {
        Task<LeaveRequest> ApplyLeaveAsync(LeaveRequest request);
        Task<LeaveRequest> ApproveLeaveAsync(int leaveRequestId);
        Task<IEnumerable<LeaveRequest>> GetByEmpAsync(int empId);
        Task<IEnumerable<LeaveSummaryDto>> GetLeaveSummaryAsync(int empId);
        Task<IEnumerable<LeaveTypeSummaryDto>> GetLeaveTypeSummaryAsync(int empId);
       

    }
}
