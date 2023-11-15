namespace Shared.Repository.NoSql
{
    public sealed class MongoDbSettings<TEntity>
    {
        public string ConnectionURI { get; set; }

        public string DatabaseName { get; set; }

        public string CollectionName { get; set; }
    }
}
