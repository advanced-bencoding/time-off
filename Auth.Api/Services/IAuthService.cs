using Auth.Api.DTOs;

namespace Auth.Api.Services;

public interface IAuthService
{
    Task<Result<RegisterUserDTO>> RegisterAsync(RegisterUserDTO userDTO, CancellationToken cancellationToken);
    Task<Result<LoginResponseDTO>> LoginAsync(LoginDTO loginDTO, CancellationToken cancellationToken); 
    Task<Result<LoginResponseDTO>> RefreshAsync (string refreshToken, CancellationToken cancellationToken);
    Task<Result<bool>> LogoutAsync(string refreshToken);
}
