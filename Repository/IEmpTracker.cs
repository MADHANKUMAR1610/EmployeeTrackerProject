using System.Linq.Expressions;

namespace EmployeeTracker.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task UpdateAsync(T entity);
        void Update(T entity);

        Task<int> SaveChangesAsync();
    }
}
