using System.Net;
using System.Text.Json;

namespace JWTAuthService.Middleware;

// ============================================================
//  GLOBAL EXCEPTION MIDDLEWARE
//  ----------------------------
//  Catches all unhandled exceptions and returns a consistent
//  JSON error response. Must be registered FIRST in Program.cs.
//
//  HTTP STATUS CODE MAPPING:
//  -------------------------
//  InvalidOperationException  → 409 Conflict  (e.g. duplicate email)
//  UnauthorizedAccessException → 401 Unauthorized
//  ArgumentException          → 400 Bad Request
//  KeyNotFoundException        → 404 Not Found
//  (all others)               → 500 Internal Server Error
// ============================================================

/// <summary>
/// Middleware that catches all unhandled exceptions and returns
/// a structured JSON error response with the correct HTTP status code.
/// </summary>
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
            _logger.LogError(ex, "Unhandled exception — {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            InvalidOperationException ex  => (HttpStatusCode.Conflict,           ex.Message),
            UnauthorizedAccessException ex => (HttpStatusCode.Unauthorized,       ex.Message),
            ArgumentException ex          => (HttpStatusCode.BadRequest,          ex.Message),
            KeyNotFoundException ex       => (HttpStatusCode.NotFound,            ex.Message),
            _                             => (HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.")
        };

        var errorResponse = new
        {
            success    = false,
            statusCode = (int)statusCode,
            message,
            timestamp  = DateTime.UtcNow,
            path       = context.Request.Path.Value
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode  = (int)statusCode;

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(errorResponse,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}
