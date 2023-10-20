using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.DBContext;

namespace TeamHub.DAL.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class, new()
{
    protected readonly TeamHubDbContext TeamHubDbContext;

    protected Repository(TeamHubDbContext socialNetworkContext)
    {
        TeamHubDbContext = socialNetworkContext;
    }

    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return await TeamHubDbContext.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> where)
    {
        return await TeamHubDbContext.Set<TEntity>().Where(where).ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        return await TeamHubDbContext.Set<TEntity>().FindAsync(id);
    }

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> where)
    {
        return await TeamHubDbContext.Set<TEntity>().FirstOrDefaultAsync(where);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        return (await TeamHubDbContext.AddAsync(entity)).Entity;
    }

    public virtual void Update(TEntity entity)
    {
        TeamHubDbContext.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Delete(TEntity entity)
    {
        TeamHubDbContext.Set<TEntity>().Remove(entity);
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
        TeamHubDbContext.Set<TEntity>().RemoveRange(entities);
    }

    public virtual async Task SaveAsync()
    {
        await TeamHubDbContext.SaveChangesAsync();
    }
}
