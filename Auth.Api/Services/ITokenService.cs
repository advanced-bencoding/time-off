using Auth.Api.Models;

namespace Auth.Api.Services;

public interface ITokenService
{
    public string GenerateToken(User user);
}
