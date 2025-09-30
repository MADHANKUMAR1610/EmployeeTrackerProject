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
builder.Services.AddScoped<IAttendanceCalendarService, AttendanceCalendarService>();


builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173/") // your React app URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();
// Use CORS before app.UseAuthorization()
app.UseCors("AllowReactApp"); 


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
