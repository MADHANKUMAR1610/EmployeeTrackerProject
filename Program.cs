using EmployeeTracker.Datas;
using EmployeeTracker.Repository;

using EmployeeTracker.Services;
using Microsoft.EntityFrameworkCore;
using System;
using static EmployeeTracker.Repository.EmpTracker;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
builder.Services.AddDbContext<EmployeeTrackerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IWorkSessionService, WorkSessionService>();
builder.Services.AddScoped<IBreakService, BreakService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ILeaveService, LeaveService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations & seed
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<EmployeeTrackerDbContext>();
    ctx.Database.Migrate();

    // Simple seeding
    if (!ctx.Employees.Any())
    {
        var emp = new EmployeeTracker.Models.Employee
        {
            Name = "Default User",
            Mail = "user@example.com",
            Password = "Test@123",
            Role = "Developer",
            ProfilePictureUrl = null
        };
        ctx.Employees.Add(emp);
        ctx.SaveChanges();

        // seed some leave balances
        ctx.LeaveBalances.Add(new EmployeeTracker.Models.LeaveBalance
        {
            EmpId = emp.Id,
            LeaveType = EmployeeTracker.Models.LeaveType.Casual,
            TotalLeave = 12,
            UsedLeave = 0
        });
        ctx.LeaveBalances.Add(new EmployeeTracker.Models.LeaveBalance
        {
            EmpId = emp.Id,
            LeaveType = EmployeeTracker.Models.LeaveType.Medical,
            TotalLeave = 12,
            UsedLeave = 0
        });
        ctx.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
