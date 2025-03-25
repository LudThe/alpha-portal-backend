using System.Linq.Expressions;

namespace Data.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<bool> AddAsync(TEntity entity);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> selector);
        Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<TEntity, T>> selector);
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<T?> GetAsync<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> selector);
        Task<bool> RemoveAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
    }
}