using System.Net;
using System.Text.Json;
using FluentValidation;

namespace FinancingLead.Web.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        object problemDetails;
        int statusCode;

        switch (exception)
        {
            case ValidationException validationEx:
                statusCode = (int)HttpStatusCode.BadRequest;
                problemDetails = new
                {
                    title = "Validation Failed",
                    status = statusCode,
                    detail = string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage)),
                    errors = validationEx.Errors.GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                };
                break;

            case ArgumentException argEx:
                statusCode = (int)HttpStatusCode.BadRequest;
                problemDetails = new
                {
                    title = "Invalid Argument",
                    status = statusCode,
                    detail = argEx.Message
                };
                break;

            case InvalidOperationException opEx:
                statusCode = (int)HttpStatusCode.BadRequest;
                problemDetails = new
                {
                    title = "Invalid Operation",
                    status = statusCode,
                    detail = opEx.Message
                };
                break;

            default:
                statusCode = (int)HttpStatusCode.InternalServerError;
                problemDetails = new
                {
                    title = "An error occurred",
                    status = statusCode,
                    detail = "An unexpected error occurred"
                };
                break;
        }

        context.Response.StatusCode = statusCode;

        var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}