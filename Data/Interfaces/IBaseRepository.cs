﻿using System.Linq.Expressions;

namespace Data.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<bool> AddAsync(TEntity entity);
        Task<bool> RemoveAsync(TEntity entity);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? filterBy = null, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> findBy, params Expression<Func<TEntity, object>>[] includes);
        Task<bool> UpdateAsync(TEntity entity);
    }
}