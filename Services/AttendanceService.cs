using EmployeeTracker.Datas;
using EmployeeTracker.Models;
using EmployeeTracker.Repository;
using System;

namespace EmployeeTracker.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly EmployeeTrackerDbContext _ctx;
        public AttendanceService(EmployeeTrackerDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<Attendance>> GetAttendanceByEmpAsync(int empId)
        {
            return await Task.FromResult(_ctx.Attendances.Where(a => a.EmpId == empId).OrderByDescending(a => a.Date));
        }
    }
    }

