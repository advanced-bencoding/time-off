
using StackExchange.Redis;

namespace Auth.Api.Repositories;

public class RefreshTokenStore : IRefreshTokenStore
{
    private readonly IDatabase _redisStore;
    public RefreshTokenStore(IConnectionMultiplexer redis)
    {
        _redisStore = redis.GetDatabase();
    }

    private static string GetKey(string tokenId) => $"refresh:{tokenId}";
    public async Task<Guid?> GetUserIdAsync(string tokenId)
    {
        var value = await _redisStore.StringGetAsync(GetKey(tokenId));
        if (value == RedisValue.Null) return null;
        return Guid.TryParse(value, out var userId) ? userId : null;
    }

    public async Task RemoveAsync(string tokenId)
    {
        await _redisStore.KeyDeleteAsync(GetKey(tokenId));
    }

    public async Task StoreAsync(string tokenId, Guid userId, TimeSpan ttl)
    {
        await _redisStore.StringSetAsync(GetKey(tokenId), userId.ToString(), ttl);
    }
}
