using Auth.Api.DTOs;
using Auth.Api.Models;
using Auth.Api.Repositories;

namespace Auth.Api.Services;

public class AuthService(IUserRepository userRepository) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    public async Task<Result<RegisterUserDTO>> RegisterAsync(RegisterUserDTO userDTO, CancellationToken cancellationToken)
    {
        if (await _userRepository.EmailExistsAsync(userDTO.Email, cancellationToken))
        {
            return Result<RegisterUserDTO>.Failure("User with given email already exists.");
        }
        await _userRepository.AddAsync(new User()
        {
            Email = userDTO.Email,
            FirstName = userDTO.FirstName,
            LastName = userDTO.LastName,
            MiddleName = userDTO.MiddleName,
            PasswordHash = userDTO.PasswordHash,
            UserId = Guid.NewGuid(),
        }, cancellationToken);

        return Result<RegisterUserDTO>.Success(userDTO);
    }
}
