namespace Shared.Repository.NoSql.Redis
{
    public interface IRedisRepository
    {
        T GetData<T>(string key);
        bool SetData<T>(string key, T value, TimeSpan expirationTime);
        object RemoveData(string key);
    }
}
