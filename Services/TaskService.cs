using EmployeeTracker.Datas;
using EmployeeTracker.Models;


namespace EmployeeTracker.Services
{
    public class TaskService : ITaskService
    {
        private readonly EmployeeTrackerDbContext _ctx;
        public TaskService(EmployeeTrackerDbContext ctx) => _ctx = ctx;

        public async Task<EmpTask> CreateTaskAsync(EmpTask t)
        {
            _ctx.EmplTasks.Add(t);
            await _ctx.SaveChangesAsync();
            return t;
        }

        public async Task<IEnumerable<EmpTask>> GetByEmpAsync(int empId)
        {
            return await Task.FromResult(_ctx.EmplTasks.Where(x => x.EmpId == empId || x.AssigneeId == empId).AsEnumerable());
        }
    }
}
