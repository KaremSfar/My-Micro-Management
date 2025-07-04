using StackExchange.Redis;

public class UserConnectionRepository : IUserConnectionRepository
{
    private readonly IDatabase _db;
    private const string KeyPrefix = "user:connections:";

    public UserConnectionRepository(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public Task<int> GetActiveConnectionsAsync(Guid userId)
        => _db.StringGetAsync(KeyPrefix + userId).ContinueWith(t => (int)(t.Result.IsNull ? 0 : (int)t.Result));

    public Task SetActiveConnectionsAsync(Guid userId, int count)
        => _db.StringSetAsync(KeyPrefix + userId, count);

    public Task IncrementActiveConnectionsAsync(Guid userId)
        => _db.StringIncrementAsync(KeyPrefix + userId);

    public Task DecrementActiveConnectionsAsync(Guid userId)
        => _db.StringDecrementAsync(KeyPrefix + userId);

    public async Task<IEnumerable<(Guid UserId, int Count)>> GetAllInactiveUsersAsync()
    {
        var server = _db.Multiplexer.GetServer(_db.Multiplexer.GetEndPoints().First());
        var result = new List<(Guid UserId, int Count)>();

        await foreach (var key in server.KeysAsync(pattern: KeyPrefix + "*"))
        {
            var value = await _db.StringGetAsync(key);
            if (value.HasValue && int.TryParse(value, out int count) && count == 0)
            {
                var userIdStr = key.ToString().Substring(KeyPrefix.Length);
                if (Guid.TryParse(userIdStr, out Guid userId))
                {
                    result.Add((userId, count));
                }
            }
        }

        return result;
    }
}