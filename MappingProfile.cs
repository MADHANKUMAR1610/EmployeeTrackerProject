using AutoMapper;
using EmployeeTracker.Models;
using EmployeeTracker.Dtos;
namespace EmployeeTracker
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Employee
            CreateMap<Employee, EmployeeDto>();
            CreateMap<CreateEmployeeDto, Employee>();

            // WorkSession
            CreateMap<WorkSession, WorkSessionDto>();
            CreateMap<CreateWorkSessionDto, WorkSession>();

     
            // DTO → Model
            CreateMap<CreateLeaveRequestDto, LeaveRequest>()
                .ForMember(dest => dest.LeaveType,
                           opt => opt.MapFrom(src => Enum.Parse<LeaveType>(src.LeaveType, true))); // 👈 parse string → enum

            // Model → DTO
            CreateMap<LeaveRequest, LeaveRequestDto>()
                .ForMember(dest => dest.LeaveType,
                           opt => opt.MapFrom(src => src.LeaveType.ToString())); // 👈 enum → string
        

        // Break
        CreateMap<Break, BreakDto>();
            CreateMap<CreateBreakDto, Break>();

            // Task
            CreateMap<EmpTask, EmpTaskDto>();
            CreateMap<CreateEmpTaskDto, EmpTask>();

            // Leave Request
            CreateMap<LeaveRequest, LeaveRequestDto>();
            CreateMap<CreateLeaveRequestDto, LeaveRequest>();

            // Leave Balance
            CreateMap<LeaveBalance, LeaveBalanceDto>();
            CreateMap<CreateLeaveBalanceDto, LeaveBalance>();
        }
    }
}

