using System.Linq.Expressions;

namespace Shared.Repository.Sql;

public interface IRepository<TEntity>
    where TEntity : class, new()
{
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetAllAsync(
        int offset,
        int limit,
        CancellationToken cancellationToken = default
    );

    Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> where,
        int offset,
        int limit,
        CancellationToken cancellationToken
    );
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> where,
        CancellationToken cancellationToken
    );
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<TEntity?> DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task SaveAsync(CancellationToken cancellationToken = default);
}
