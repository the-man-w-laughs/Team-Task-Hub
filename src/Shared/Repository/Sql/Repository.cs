using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Shared.Repository.Sql;

public abstract class Repository<TDbContext, TEntity> : IRepository<TEntity>
    where TEntity : class, new()
    where TDbContext : DbContext
{
    protected readonly TDbContext DbContext;
    private readonly DbSet<TEntity> _table;

    protected Repository(TDbContext dbContext)
    {
        DbContext = dbContext;
        _table = dbContext.Set<TEntity>();
    }

    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return await _table.ToListAsync();
    }

    public virtual async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> where)
    {
        return await _table.Where(where).ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _table.FindAsync(id);
    }

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> where)
    {
        return await _table.FirstOrDefaultAsync(where);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        return (await DbContext.AddAsync(entity)).Entity;
    }

    public virtual void Update(TEntity entity)
    {
        DbContext.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Delete(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
    }

    public virtual async Task<TEntity?> DeleteByIdAsync(int id)
    {
        var entity = await GetByIdAsync(id);

        if (entity != null)
        {
            Delete(entity);
        }

        return entity;
    }

    public virtual async Task DeleteRangeAsync(Expression<Func<TEntity, bool>> where)
    {
        var entities = await GetAllAsync(where);
        _table.RemoveRange(entities);
    }

    public virtual async Task SaveAsync()
    {
        await DbContext.SaveChangesAsync();
    }
}
