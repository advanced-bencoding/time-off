using Auth.Api.DTOs;
using Auth.Api.Models;
using Auth.Api.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Services;

public class AuthService(IUserRepository userRepository, ITokenService tokenService) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public async Task<Result<string>> LoginAsync(LoginDTO loginDTO, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmail(loginDTO.EmailId, cancellationToken);
        if (user == null) return Result<string>.Failure("Invalid email or password");
        
        var passwordVerification = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDTO.Password);
        if (passwordVerification == PasswordVerificationResult.Failed) return Result<string>.Failure("Invalid email or password");

        var accessToken = _tokenService.GenerateToken(user);
        return Result<string>.Success(accessToken);
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
        user.PasswordHash = _passwordHasher.HashPassword(user, userDTO.PasswordHash);

        await _userRepository.AddAsync(user, cancellationToken);

        return Result<RegisterUserDTO>.Success(userDTO);
    }
}
