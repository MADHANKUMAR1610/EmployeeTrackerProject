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

            // Attendance
            CreateMap<Attendance, AttandanceDto>();
            CreateMap<CreateAttendanceDto, Attendance>();

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

