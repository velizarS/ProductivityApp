using System.Linq.Expressions;

namespace ProductivityApp.Data.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        IQueryable<T> Query(); 
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}