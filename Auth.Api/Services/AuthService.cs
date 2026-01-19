using Auth.Api.DTOs;
using Auth.Api.Models;
using Auth.Api.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Auth.Api.Services;

public class AuthService(IUserRepository userRepository, ITokenService tokenService, IRefreshTokenStore refreshTokenStore, IOptions<RefreshTokenConfig> refreshTokenOptions) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IRefreshTokenStore _refreshTokenStore = refreshTokenStore;
    private readonly PasswordHasher<User> _passwordHasher = new();
    private readonly RefreshTokenConfig _refreshTokenConfig = refreshTokenOptions.Value;

    public async Task<Result<LoginResponseDTO>> LoginAsync(LoginDTO loginDTO, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(loginDTO.EmailId, cancellationToken);
        if (user == null) return Result<LoginResponseDTO>.Failure("Invalid email or password");
        
        var passwordVerification = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDTO.Password);
        if (passwordVerification == PasswordVerificationResult.Failed) return Result<LoginResponseDTO>.Failure("Invalid email or password");

        var accessToken = _tokenService.GenerateToken(user);
        var refreshToken = Guid.NewGuid().ToString();

        await _refreshTokenStore.StoreAsync(refreshToken, user.UserId, TimeSpan.FromDays(_refreshTokenConfig.ExpiryDays));

        return Result<LoginResponseDTO>.Success(new LoginResponseDTO(accessToken, refreshToken));
    }

    public async Task<Result<LoginResponseDTO>> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var userId = await _refreshTokenStore.GetUserIdAsync(refreshToken);
        if (userId == null) return Result<LoginResponseDTO>.Failure("Invalid or expired refresh token");

        await _refreshTokenStore.RemoveAsync(refreshToken);

        var user = await _userRepository.GetByIdAsync(userId.Value, cancellationToken);
        if (user == null) return Result<LoginResponseDTO>.Failure("Invalid or expired refresh token");

        var accessToken = _tokenService.GenerateToken(user);
        var refreshTokenNew = Guid.NewGuid().ToString();

        await _refreshTokenStore.StoreAsync(refreshTokenNew, user.UserId, TimeSpan.FromDays(_refreshTokenConfig.ExpiryDays));

        return Result<LoginResponseDTO>.Success(new LoginResponseDTO(accessToken, refreshToken));
    }

    public async Task<Result<RegisterUserDTO>> RegisterAsync(RegisterUserDTO userDTO, CancellationToken cancellationToken)
    {
        if (await _userRepository.EmailExistsAsync(userDTO.Email, cancellationToken))
        {
            return Result<RegisterUserDTO>.Failure("User with given email already exists.");
        }

        var user = new User()
        {
            Email = userDTO.Email,
            FirstName = userDTO.FirstName,
            LastName = userDTO.LastName,
            MiddleName = userDTO.MiddleName,
            UserId = Guid.NewGuid(),
            Role = "employee"            
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, userDTO.Password);

        await _userRepository.AddAsync(user, cancellationToken);

        return Result<RegisterUserDTO>.Success(userDTO);
    }

    public async Task<Result<bool>> LogoutAsync(string refreshToken)
    {
        await _refreshTokenStore.RemoveAsync(refreshToken);
        return Result<bool>.Success(true);
    }
}
