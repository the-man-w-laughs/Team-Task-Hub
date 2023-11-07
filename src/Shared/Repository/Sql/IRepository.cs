using System.Linq.Expressions;

namespace Shared.Repository.Sql;

public interface IRepository<TEntity>
    where TEntity : class, new()
{
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> where,
        CancellationToken cancellationToken
    );
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> where,
        CancellationToken cancellationToken
    );
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<TEntity?> DeleteByIdAsync(int id, CancellationToken cancellationToken);
    Task SaveAsync(CancellationToken cancellationToken);
}
