using Data.Contexts;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;


public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly DataContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected BaseRepository(DataContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }


    public virtual async Task<bool> AddAsync(TEntity entity)
    {
        if (entity == null)
            return false;

        try
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public virtual async Task<bool> RemoveAsync(TEntity entity)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(entity);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }



    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? filterBy = null, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filterBy != null)
            query = query.Where(filterBy);

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        if (sortBy != null)
            query = orderByDescending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);

        return await query.ToListAsync();
    }

    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> findBy, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        var entity = await query.FirstOrDefaultAsync(findBy);
        return entity ?? null!;
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            return false;

        try
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }
}