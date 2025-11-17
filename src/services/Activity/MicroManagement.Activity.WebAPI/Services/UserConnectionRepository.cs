using StackExchange.Redis;
using System.Globalization;
using System.Runtime.Serialization;

public class UserConnectionRepository : IUserConnectionRepository
{
    private readonly IDatabase _db;
    private const string KeyPrefix = "user:connections:";
    private const string DateSuffix = ":updatedAt";

    public UserConnectionRepository(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public Task<int> GetActiveConnectionsAsync(Guid userId)
        => _db.StringGetAsync(KeyPrefix + userId).ContinueWith(t => (int)(t.Result.IsNull ? 0 : (int)t.Result));

    public async Task SetActiveConnectionsAsync(Guid userId, int count)
    {
        var tasks = new Task[]
        {
            _db.StringSetAsync(KeyPrefix + userId, count),
            _db.StringSetAsync(KeyPrefix + userId + DateSuffix, DateTime.UtcNow.ToString("o"))
        };
        await Task.WhenAll(tasks);
    }

    public async Task IncrementActiveConnectionsAsync(Guid userId)
    {
        var tasks = new Task[]
        {
            _db.StringIncrementAsync(KeyPrefix + userId),
            _db.StringSetAsync(KeyPrefix + userId + DateSuffix, DateTime.UtcNow.ToString("o"))
        };
        await Task.WhenAll(tasks);
    }

    public async Task DecrementActiveConnectionsAsync(Guid userId)
    {
        var tasks = new Task[]
        {
            _db.StringDecrementAsync(KeyPrefix + userId),
            _db.StringSetAsync(KeyPrefix + userId + DateSuffix, DateTime.UtcNow.ToString("o"))
        };
        await Task.WhenAll(tasks);
    }

    public async Task<DateTime?> GetLastUpdateAsync(Guid userId)
    {
        var value = await _db.StringGetAsync(KeyPrefix + userId + DateSuffix);
        if (value.HasValue && DateTime.TryParse(value, null, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var dt))
            return dt;
        return null;
    }

    public async Task<IEnumerable<(Guid UserId, int Count, DateTime? UpdatedAt)>> GetAllInactiveUsersAsync()
    {
        var server = _db.Multiplexer.GetServer(_db.Multiplexer.GetEndPoints().First());
        var result = new List<(Guid UserId, int Count, DateTime? UpdatedAt)>();

        // Collect all user connection keys (excluding updatedAt keys)
        var userKeys = new List<RedisKey>();
        await foreach (var key in server.KeysAsync(pattern: KeyPrefix + "*"))
        {
            var keyStr = key.ToString();
            if (!keyStr.EndsWith(DateSuffix, StringComparison.Ordinal))
            {
                userKeys.Add(key);
            }
        }

        // Batch fetch all values for efficiency
        var values = await _db.StringGetAsync(userKeys.ToArray());

        for (int i = 0; i < userKeys.Count; i++)
        {
            var value = values[i];
            if (value.HasValue && value.TryParse(out int count) && count == 0)
            {
                var userIdStr = userKeys[i].ToString().Substring(KeyPrefix.Length);
                if (Guid.TryParse(userIdStr, out Guid userId))
                {
                    var updatedAt = await GetLastUpdateAsync(userId);
                    result.Add((userId, count, updatedAt));
                }
            }
        }

        return result;
    }
}