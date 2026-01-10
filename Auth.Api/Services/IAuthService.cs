using Auth.Api.DTOs;

namespace Auth.Api.Services;

public interface IAuthService
{
    Task<Result<RegisterUserDTO>> RegisterAsync(RegisterUserDTO userDTO, CancellationToken cancellationToken);
}
