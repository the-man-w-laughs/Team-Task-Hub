using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Shared.Repository.NoSql
{
    public class MongoRepository<TEntity> : IMongoRepository<TEntity>
        where TEntity : MongoBaseEntity
    {
        private readonly IMongoCollection<TEntity> _mongoCollection;

        public MongoRepository(
            IOptions<MongoDbSettings<TEntity>> mongoSettings,
            IMongoDbSeeder<TEntity> mongoDbSeeder
        )
        {
            var mongoConfig = mongoSettings.Value;
            var client = new MongoClient(mongoConfig.ConnectionURI);
            var database = client.GetDatabase(mongoConfig.DatabaseName);

            var collectionExists = CollectionExists(database, mongoConfig.CollectionName);

            _mongoCollection = database.GetCollection<TEntity>(mongoConfig.CollectionName);

            if (!collectionExists)
            {
                var seedData = mongoDbSeeder.Seed();

                if (seedData.Any())
                {
                    _mongoCollection.InsertMany(seedData);
                }
            }
        }

        public bool CollectionExists(IMongoDatabase database, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var options = new ListCollectionNamesOptions { Filter = filter };

            return database.ListCollectionNames(options).Any();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            int offset,
            int limit,
            Expression<Func<TEntity, bool>> filter = null,
            CancellationToken cancellationToken = default
        )
        {
            var query =
                filter == null
                    ? _mongoCollection.Find(Builders<TEntity>.Filter.Empty)
                    : _mongoCollection.Find(Builders<TEntity>.Filter.Where(filter));

            query = query.Skip(offset).Limit(limit);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<TEntity> GetOneAsync(
            Expression<Func<TEntity, bool>> filter,
            CancellationToken cancellationToken = default
        )
        {
            var filterDefinition = Builders<TEntity>.Filter.Where(filter);

            return await _mongoCollection
                .Find(filterDefinition)
                .Limit(1)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TEntity> GetByIdAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            var entity = await _mongoCollection.FindAsync(
                Builders<TEntity>.Filter.Eq(entity => entity.Id, id)
            );

            return await entity.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _mongoCollection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var filter = Builders<TEntity>.Filter.Eq(
                existingEntity => existingEntity.Id,
                entity.Id
            );

            await _mongoCollection.ReplaceOneAsync(
                filter,
                entity,
                cancellationToken: cancellationToken
            );
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<TEntity>.Filter.Eq(existingEntity => existingEntity.Id, id);

            await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
        }
    }
}
