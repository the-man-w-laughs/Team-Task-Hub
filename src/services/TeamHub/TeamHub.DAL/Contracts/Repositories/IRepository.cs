using System.Linq.Expressions;

namespace TeamHub.DAL.Contracts.Repositories;

public interface IRepository<TEntity>
    where TEntity : class, new()
{
    Task<List<TEntity>> GetAllAsync(int offset, int limit);
    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> where, int offset, int limit);
    Task<TEntity?> GetByIdAsync(int id);
    Task<TEntity> AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<TEntity?> DeleteByIdAsync(int id);
    Task SaveAsync();
}
