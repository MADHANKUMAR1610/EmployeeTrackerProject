using EmployeeTracker.Datas;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace EmployeeTracker.Repository
{
    public class EmpTracker
    {
        public class GenericRepository<T> : IGenericRepository<T> where T : class
        {

            protected readonly EmployeeTrackerDbContext _ctx;
            public GenericRepository(EmployeeTrackerDbContext ctx) => _ctx = ctx;

            public async Task<T> AddAsync(T entity)
            {
                var e = (await _ctx.Set<T>().AddAsync(entity)).Entity;
                await _ctx.SaveChangesAsync();
                return e;
            }

            public async Task DeleteAsync(int id)
            {
                var set = _ctx.Set<T>();
                var entity = await set.FindAsync(id);
                if (entity == null) return;
                set.Remove(entity);
                await _ctx.SaveChangesAsync();
            }

            public async Task<IEnumerable<T>> GetAllAsync()
            {
                return await _ctx.Set<T>().ToListAsync();
            }

            public async Task<T> GetAsync(int id) => await _ctx.Set<T>().FindAsync(id);

            public async Task UpdateAsync(T entity)
            {
                _ctx.Set<T>().Update(entity);
                await _ctx.SaveChangesAsync();
            }
        }
    }
}
