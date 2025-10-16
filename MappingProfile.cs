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
            CreateMap<CreateLeaveRequestDto, LeaveRequest>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => LeaveStatus.Approved));
            CreateMap<LeaveRequest, LeaveRequestDto>();

            // ---------------- Leave Balance ----------------
            CreateMap<LeaveBalance, LeaveBalanceDto>();
            CreateMap<CreateLeaveBalanceDto, LeaveBalance>();

            // ---------------- Break ----------------
            CreateMap<Break, BreakDto>();
            CreateMap<CreateBreakDto, Break>();

            // ---------------- EmpTask ----------------
            // Create mapping: CreateEmpTaskDto -> EmpTask
            CreateMap<CreateEmpTaskDto, EmpTask>()
                .ForMember(dest => dest.Priority,
                    opt => opt.MapFrom(src =>
                        !string.IsNullOrEmpty(src.Priority)
                            ? (TaskPriority)Enum.Parse(typeof(TaskPriority), src.Priority, true)
                            : TaskPriority.Medium))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src =>
                        !string.IsNullOrEmpty(src.Status)
                            ? (TaskStatus)Enum.Parse(typeof(TaskStatus), src.Status, true)
                            : TaskStatus.Pending));




            // Entity -> DTO mapping: EmpTask -> EmpTaskDto
            CreateMap<EmpTask, EmpTaskDto>()
                .ForMember(dest => dest.AssigneeName,
                           opt => opt.MapFrom(src => src.Assignee != null ? src.Assignee.Name : ""))
                .ForMember(dest => dest.Priority,
                           opt => opt.MapFrom(src => src.Priority.ToString()))
                .ForMember(dest => dest.Status,
                           opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
