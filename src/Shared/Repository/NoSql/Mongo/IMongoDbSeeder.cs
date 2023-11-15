namespace Shared.Repository.NoSql
{
    public interface IMongoDbSeeder<TEntity>
    {
        IEnumerable<TEntity> Seed();
    }
}
