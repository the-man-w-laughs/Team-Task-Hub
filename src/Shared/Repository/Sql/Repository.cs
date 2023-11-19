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

    public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetAllAsync(
        int offset,
        int limit,
        CancellationToken cancellationToken = default
    )
    {
        return await DbContext
            .Set<TEntity>()
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> where,
        int offset,
        int limit,
        CancellationToken cancellationToken = default
    )
    {
        return await DbContext
            .Set<TEntity>()
            .Where(where)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        return await _table.FindAsync(id, cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> where,
        CancellationToken cancellationToken = default
    )
    {
        return await _table.FirstOrDefaultAsync(where, cancellationToken);
    }

    public async Task<TEntity> AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        return (await DbContext.AddAsync(entity, cancellationToken)).Entity;
    }

    public virtual void Update(TEntity entity)
    {
        DbContext.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Delete(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
    }

    public virtual async Task<TEntity?> DeleteByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await GetByIdAsync(id, cancellationToken);

        if (entity != null)
        {
            Delete(entity);
        }

        return entity;
    }

    public virtual async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}
