namespace WebApiLab05.Exceptions;

// ============================================================
//  CUSTOM EXCEPTIONS
//  ------------------
//  Custom exception types let the global middleware map each
//  exception to the correct HTTP status code automatically.
//
//  NotFoundException      → 404 Not Found
//  ValidationException    → 400 Bad Request
//  ConflictException      → 409 Conflict
//  (all others)           → 500 Internal Server Error
// ============================================================

/// <summary>
/// Thrown when a requested resource does not exist.
/// Maps to HTTP 404 Not Found.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string resourceName, int id)
        : base($"{resourceName} with Id={id} was not found.") { }
}

/// <summary>
/// Thrown when business rule validation fails.
/// Maps to HTTP 400 Bad Request.
/// </summary>
public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}

/// <summary>
/// Thrown when a resource conflict occurs (e.g. duplicate name).
/// Maps to HTTP 409 Conflict.
/// </summary>
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}
