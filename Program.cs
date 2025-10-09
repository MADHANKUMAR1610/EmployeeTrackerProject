using EmployeeTracker.Datas;
using EmployeeTracker.Repository;

using EmployeeTracker.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
builder.Services.AddDbContext<EmployeeTrackerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// CORS - allow React dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocal", policy =>
        policy.WithOrigins("http://localhost:5173") // React dev
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

// DI
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IWorkSessionService, WorkSessionService>();
builder.Services.AddScoped<IBreakService, BreakService>();

builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddScoped<ILeaveService, LeaveService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAttendanceCalendarService, AttendanceCalendarService>();


builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowLocal");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
