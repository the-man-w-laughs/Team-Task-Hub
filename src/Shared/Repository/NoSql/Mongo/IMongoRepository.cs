using System.Linq.Expressions;

namespace Shared.Repository.NoSql
{
    public interface IMongoRepository<TEntity>
        where TEntity : MongoBaseEntity
    {
        public Task<IEnumerable<TEntity>> GetAllAsync(
            int offset,
            int limit,
            Expression<Func<TEntity, bool>> filter = null,
            CancellationToken cancellationToken = default
        );
        Task<TEntity> GetOneAsync(
            Expression<Func<TEntity, bool>> filter,
            CancellationToken cancellationToken = default
        );

        Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(string id, CancellationToken cancellationToken = default);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}
