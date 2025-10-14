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


<<<<<<< HEAD
=======
            CreateMap<CreateEmpTaskDto, EmpTask>()
                .ForMember(dest => dest.Priority,
                           opt => opt.MapFrom(src => Enum.Parse<TaskPriority>(src.Priority, true))) // string → enum
                .ForMember(dest => dest.Status,
                           opt => opt.MapFrom(src => Enum.Parse<TaskStatus>(src.Status, true)));   // string → enum
            CreateMap<EmpTask, EmpTaskDto>()
    .ForMember(dest => dest.AssigneeName,
               opt => opt.MapFrom(src => src.Assignee != null ? src.Assignee.Name : ""))
    .ForMember(dest => dest.Priority,
               opt => opt.MapFrom(src => src.Priority.ToString()))
    .ForMember(dest => dest.Status,
               opt => opt.MapFrom(src => src.Status.ToString()));
>>>>>>> 88eaa3b7d22b183668f75ccc3d7d34b05f30e1a4
        }
    }
}
