using Auth.Api.DTOs;

namespace Auth.Api.Utilities;

public static class ControllerHelper
{
    public static ApiResponse<T> MapServiceResultToApiResponse<T>(Result<T> result)
    {
        return new ApiResponse<T>()
        {
            Data = result.Data,
            Error = result.ErrorMessage,
            IsSuccess = result.IsSuccess,
        };
    }
}
