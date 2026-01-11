using Auth.Api.DTOs;
using Auth.Api.Services;
using Auth.Api.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[ApiController]
[Route("api/auth/v1")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDTO request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request, cancellationToken);
        var apiResponse = ControllerHelper.MapServiceResultToApiResponse(result);
        if (apiResponse.IsSuccess) return Ok(apiResponse);
        return BadRequest(apiResponse);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO loginDTO, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(loginDTO, cancellationToken);
        var apiResponse = ControllerHelper.MapServiceResultToApiResponse(result);
        if (apiResponse.IsSuccess) return Ok(apiResponse);
        return Unauthorized(apiResponse);
    }
}
