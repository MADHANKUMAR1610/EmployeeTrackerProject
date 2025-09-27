using System.Linq.Expressions;

namespace EmployeeTracker.Repository
{
    public interface IGenericRepository<T> where T : class
        {
            Task<IEnumerable<T>> GetAllAsync();
            Task<T> GetAsync(int id);
            Task<T> AddAsync(T entity);
            Task UpdateAsync(T entity);
            Task DeleteAsync(int id);
        }
    }

