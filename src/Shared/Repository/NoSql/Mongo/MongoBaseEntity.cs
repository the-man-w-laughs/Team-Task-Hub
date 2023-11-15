using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Repository.NoSql
{
    public abstract class MongoBaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
