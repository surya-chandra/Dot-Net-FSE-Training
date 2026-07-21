using System.Net;
using System.Text.Json;
using WebApiLab05.Exceptions;

namespace WebApiLab05.Middleware;





















public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {

            await _next(context);
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Unhandled exception for {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {

        var (statusCode, message) = exception switch
        {
            NotFoundException  ex => (HttpStatusCode.NotFound,           ex.Message),
            ValidationException ex => (HttpStatusCode.BadRequest,        ex.Message),
            ConflictException  ex => (HttpStatusCode.Conflict,           ex.Message),
            ArgumentException  ex => (HttpStatusCode.BadRequest,         ex.Message),
            _                     => (HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.")
        };

        var errorResponse = new
        {
            statusCode = (int)statusCode,
            message,
            timestamp  = DateTime.UtcNow,
            path       = context.Request.Path.Value
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode  = (int)statusCode;

        var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
