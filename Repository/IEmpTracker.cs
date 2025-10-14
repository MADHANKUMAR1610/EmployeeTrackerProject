using System.Linq.Expressions;

namespace EmployeeTracker.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Query();

        // CRUD operations
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        void Update(T entity); // optional — keep only one if not needed both
        Task DeleteAsync(int id);

        // Retrieval methods
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Utility
        Task<int> SaveChangesAsync();

        // Domain-specific (if you use Tasks table)
        Task<IEnumerable<T>> GetPendingTasksByAssigneeAsync(int assigneeId);
    }
}
