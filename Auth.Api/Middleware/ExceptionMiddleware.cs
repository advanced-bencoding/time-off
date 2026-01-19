using Auth.Api.DTOs;

namespace Auth.Api.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException)
        {
            context.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;
        }
        catch (Exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var response = new ApiResponse<bool>() { IsSuccess = false, Error = "An unexpected error occured" };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
