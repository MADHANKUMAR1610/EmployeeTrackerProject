using AutoMapper;
using EmployeeTracker.Models;
using EmployeeTracker.Dtos;
using TaskStatus = EmployeeTracker.Models.TaskStatus;
using TaskPriority = EmployeeTracker.Models.TaskPriority;

namespace EmployeeTracker
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ---------------- Employee ----------------
            CreateMap<Employee, EmployeeDto>();
            CreateMap<CreateEmployeeDto, Employee>();

            // ---------------- WorkSession ----------------
            CreateMap<WorkSession, WorkSessionDto>();
            CreateMap<CreateWorkSessionDto, WorkSession>();

            // ---------------- Leave Request ----------------
            // DTO → Model
            CreateMap<CreateLeaveRequestDto, LeaveRequest>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => LeaveStatus.Approved)); // ✅ auto-approved

            // Model → DTO
            CreateMap<LeaveRequest, LeaveRequestDto>();

            // ---------------- Leave Balance ----------------
            CreateMap<LeaveBalance, LeaveBalanceDto>();
            CreateMap<CreateLeaveBalanceDto, LeaveBalance>();

            // ---------------- Break ----------------
            CreateMap<Break, BreakDto>();
            CreateMap<CreateBreakDto, Break>();


        }
    }
}
