using System.Net;
using System.Text.Json;
using WebApiLab05.Exceptions;

namespace WebApiLab05.Middleware;

// ============================================================
//  GLOBAL EXCEPTION HANDLING MIDDLEWARE
//  --------------------------------------
//  Instead of wrapping every controller action in try-catch,
//  we register a single middleware that catches ALL unhandled
//  exceptions and returns a consistent JSON error response.
//
//  BENEFITS:
//  - Single place to handle all errors
//  - Consistent error response format across the entire API
//  - Controllers stay clean — no try-catch boilerplate
//  - Logs all errors in one place
//
//  MIDDLEWARE PIPELINE POSITION:
//  This must be registered FIRST in Program.cs so it wraps
//  all subsequent middleware and catches any exception.
// ============================================================

/// <summary>
/// Global exception handling middleware.
/// Catches all unhandled exceptions and returns a structured JSON error response.
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
            // Pass the request to the next middleware in the pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log the exception with full details
            _logger.LogError(ex, "Unhandled exception for {Method} {Path}",
                context.Request.Method, context.Request.Path);

            // Map the exception type to an HTTP status code
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Determine status code and user-facing message based on exception type
        var (statusCode, message) = exception switch
        {
            NotFoundException  ex => (HttpStatusCode.NotFound,           ex.Message),
            ValidationException ex => (HttpStatusCode.BadRequest,        ex.Message),
            ConflictException  ex => (HttpStatusCode.Conflict,           ex.Message),
            ArgumentException  ex => (HttpStatusCode.BadRequest,         ex.Message),
            _                     => (HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.")
        };

        // Build a consistent error response object
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
