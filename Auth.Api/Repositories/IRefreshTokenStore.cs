namespace Auth.Api.Repositories;

public interface IRefreshTokenStore
{
    Task StoreAsync(string tokenId, Guid userId, TimeSpan ttl);
    Task<Guid?> GetUserIdAsync(string tokenId);
    Task RemoveAsync(string tokenId);
}
