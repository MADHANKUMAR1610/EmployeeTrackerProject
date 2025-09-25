using EmployeeTracker.Datas;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace EmployeeTracker.Repository
{
    public class EmpTracker
    {
        public class GenericRepository<T>(EmployeeTrackerDbContext context) : IGenericRepository<T> where T : class
        {
            protected readonly EmployeeTrackerDbContext _context = context;
            protected readonly DbSet<T> _db = context.Set<T>();

            public async Task AddAsync(T entity)
            {
                await _db.AddAsync(entity);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
                var entity = await _db.FindAsync(id);
                if (entity == null) return;
                _db.Remove(entity);
                await _context.SaveChangesAsync();
            }

            public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            {
                return await _db.Where(predicate).ToListAsync();
            }

            public async Task<IEnumerable<T>> GetAllAsync()
            {
                return await _db.ToListAsync();
            }

            public DbSet<T> Get_db()
            {
                return _db;
            }

            public async Task<T?> GetByIdAsync(int id, DbSet<T> _db)
            {
                return await _db.FindAsync(id);
            }

            public async Task UpdateAsync(T entity)
            {
                _db.Update(entity);
                await _context.SaveChangesAsync();
            }

            public Task<T> GetByIdAsync(int id)
            {
                throw new NotImplementedException();
            }
        }
    }
}
