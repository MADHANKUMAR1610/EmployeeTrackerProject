using EmployeeTracker.Datas;
using EmployeeTracker.Models;
using EmployeeTracker.Repository;
using Microsoft.EntityFrameworkCore;
using System;

namespace EmployeeTracker.Services
{
    public class TaskService
    {
        private readonly IGenericRepository<EmpTask> _repo;
        private readonly EmployeeTrackerDbContext _context;

        public TaskService(IGenericRepository<EmpTask> repo, EmployeeTrackerDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public Task<IEnumerable<EmpTask>> GetAllAsync() => _repo.GetAllAsync();
        public Task<EmpTask> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<IEnumerable<EmpTask>> GetByEmployeeAsync(int employeeId) => _repo.FindAsync(t => t.EmployeeId == employeeId);
        public Task AddAsync(EmpTask t) => _repo.AddAsync(t);
        public Task UpdateAsync(EmpTask t) => _repo.UpdateAsync(t);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        // ✅ Assign a task to another employee
        public async Task<bool> AssignTaskAsync(int taskId, int assigneeId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            var employee = await _context.Employees.FindAsync(assigneeId);

            if (task == null || employee == null) return false;

            task.AssigneeId = assigneeId;
            await _context.SaveChangesAsync();
            return true;
        }
        // Mark task as completed
        public async Task<bool> MarkDoneAsync(int taskId)
        {
            var t = await _context.Tasks.FindAsync(taskId);
            if (t == null) return false;
            t.Status = "Completed";
            await _context.SaveChangesAsync();
            return true;
        }

        // Change priority
        public async Task<bool> ChangePriorityAsync(int taskId, string priority)
        {
            var t = await _context.Tasks.FindAsync(taskId);
            if (t == null) return false;
            t.Priority = priority;
            await _context.SaveChangesAsync();
            return true;
        }
        // ✅ Get tasks with Assignee details
        public async Task<IEnumerable<EmpTask>> GetTasksWithAssigneeAsync()
        {
            return await _context.Tasks
                .Include(t => t.Employee)
                .Include(t => t.Assignee)
                .ToListAsync();
        }
    }
}
