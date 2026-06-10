using System.Net;
using System.Text.Json;
using CoreApplication.Application.Common.Exceptions;

namespace CoreApplication.API.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, errors) = exception switch
        {
            ValidationException ve =>
                (HttpStatusCode.UnprocessableEntity, ve.Errors),
            NotFoundException =>
                (HttpStatusCode.NotFound, new Dictionary<string, string[]>
                    { ["error"] = [exception.Message] }),
            ForbiddenAccessException =>
                (HttpStatusCode.Forbidden, new Dictionary<string, string[]>
                    { ["error"] = [exception.Message] }),
            UnauthorizedAccessException =>
                (HttpStatusCode.Unauthorized, new Dictionary<string, string[]>
                    { ["error"] = [exception.Message] }),
            _ =>
                (HttpStatusCode.InternalServerError, new Dictionary<string, string[]>
                    { ["error"] = ["An unexpected error occurred."] })
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new { errors };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}
