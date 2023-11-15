using Newtonsoft.Json;
using StackExchange.Redis;

namespace Shared.Repository.NoSql.Redis
{
    public class RedisRepository : IRedisRepository
    {
        private IDatabase _db;

        public RedisRepository(string connectionString)
        {
            var configurationOptions = ConfigurationOptions.Parse(connectionString);
            _db = ConnectionMultiplexer.Connect(configurationOptions).GetDatabase();
        }

        public T GetData<T>(string key)
        {
            var value = _db.StringGet(key);

            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }

            return default;
        }

        public bool SetData<T>(string key, T value, TimeSpan expirationTime)
        {
            var isSet = _db.StringSet(key, JsonConvert.SerializeObject(value), expirationTime);

            return isSet;
        }

        public object RemoveData(string key)
        {
            bool _isKeyExist = _db.KeyExists(key);

            if (_isKeyExist == true)
            {
                return _db.KeyDelete(key);
            }

            return false;
        }
    }
}
