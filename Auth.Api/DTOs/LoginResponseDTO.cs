namespace Auth.Api.DTOs;

public class LoginResponseDTO(string accessToken, string refreshToken)
{
    public string AccessToken { get; } = accessToken;
    public string RefreshToken { get; } = refreshToken;
}
