

namespace FinancingLead.Application.Common;

public class BaseResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }

    public static BaseResponse<T> SuccessResponse(T data, string? message = null)
    {
        return new BaseResponse<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }

    public static BaseResponse<T> ErrorResponse(string error)
    {
        return new BaseResponse<T>
        {
            Success = false,
            Errors = new List<string> { error }
        };
    }

    public static BaseResponse<T> ErrorResponse(List<string> errors)
    {
        return new BaseResponse<T>
        {
            Success = false,
            Errors = errors
        };
    }
}