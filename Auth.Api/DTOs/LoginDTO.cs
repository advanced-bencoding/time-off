namespace Auth.Api.DTOs;

public record struct LoginDTO
(
    string EmailId,
    string Password
);
