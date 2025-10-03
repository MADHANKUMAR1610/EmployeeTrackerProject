using EmployeeTracker.Datas;
using EmployeeTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmployeeTracker.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly EmployeeTrackerDbContext _ctx;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(EmployeeTrackerDbContext ctx)
        {
            _ctx = ctx;
            _dbSet = _ctx.Set<T>();
        }
        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<T> AddAsync(T entity)
        {
            var e = (await _dbSet.AddAsync(entity)).Entity;
            await _ctx.SaveChangesAsync();   // save immediately
            return e;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _ctx.SaveChangesAsync();   // ensure changes are saved
        }
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _ctx.SaveChangesAsync();
        }

        // ------------------ New Task-specific method ------------------
        public async Task<IEnumerable<T>> GetPendingTasksByAssigneeAsync(int assigneeId)
        {
            return await _dbSet
                .Where(t => EF.Property<int>(t, "AssigneeId") == assigneeId
                         && EF.Property<string>(t, "Status") == "Pending")
                .ToListAsync();
        }

    }
}
