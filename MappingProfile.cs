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

            // ---------------- Task ----------------
            CreateMap<EmpTask, EmpTaskDto>()
                .ForMember(dest => dest.AssigneeName,
                           opt => opt.MapFrom(src => src.Assignee != null ? src.Assignee.Name : ""))
                .ForMember(dest => dest.Priority,
                           opt => opt.MapFrom(src => src.Priority.ToString())) // enum → string
                .ForMember(dest => dest.Status,
                           opt => opt.MapFrom(src => src.Status.ToString()));  // enum → string

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
            CreateMap<CreateLeaveRequestDto, LeaveRequest>()
    .ForMember(dest => dest.LeaveType, opt =>
        opt.MapFrom(src => Enum.Parse<LeaveType>(src.LeaveType, true)));

        }
    }
}
