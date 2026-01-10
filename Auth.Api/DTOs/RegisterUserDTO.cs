namespace Auth.Api.DTOs;

public record struct RegisterUserDTO
(
    string FirstName,
    string? MiddleName,
    string LastName,
    string Email,
    string PasswordHash
);
