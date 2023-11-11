namespace Shared.Repository.NoSql.Redis
{
    public class UserRequestRepository : RedisRepository, IUserRequestRepository
    {
        private readonly string _userRequestsKey = "latest_user_requests";

        public UserRequestRepository(string connectionString)
            : base(connectionString) { }

        public DateTime? GetLatestRequestTime(string userId)
        {
            var key = GetKey(userId);
            var timestampString = base.GetData<string>(key);

            if (!string.IsNullOrEmpty(timestampString))
            {
                if (DateTime.TryParse(timestampString, out DateTime timestamp))
                {
                    return timestamp;
                }
            }

            return null;
        }

        public void SetLatestRequestTime(string userId, DateTime timestamp)
        {
            var key = GetKey(userId);
            base.SetData(key, timestamp.ToString(), TimeSpan.FromSeconds(10));
        }

        public void DeleteLatestRequestTime(string userId)
        {
            var key = GetKey(userId);
            base.RemoveData(key);
        }

        private string GetKey(string userId)
        {
            return $"{_userRequestsKey}:{userId}";
        }
    }
}
