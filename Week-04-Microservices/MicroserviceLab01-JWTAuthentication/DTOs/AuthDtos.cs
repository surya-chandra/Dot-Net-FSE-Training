using System.ComponentModel.DataAnnotations;

namespace JWTAuthService.DTOs;

// ============================================================
//  DATA TRANSFER OBJECTS (DTOs)
//  -----------------------------
//  DTOs decouple the API contract from the database model.
//  The User entity is never returned directly — only DTOs.
//
//  This prevents:
//  - Exposing PasswordHash to clients
//  - Over-posting attacks
//  - Tight coupling between API and DB schema
// ============================================================

/// <summary>Request body for POST /api/auth/register.</summary>
public class RegisterRequestDto
{
    /// <example>John Doe</example>
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, MinimumLength = 2,
        ErrorMessage = "Full name must be between 2 and 100 characters.")]
    public string FullName { get; set; } = string.Empty;

    /// <example>john.doe@example.com</example>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
    [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
    public string Email { get; set; } = string.Empty;

    /// <example>SecurePass@123</example>
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6,
        ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Optional role assignment. Defaults to "User" if not provided.
    /// Only "Admin" or "User" are accepted.
    /// </summary>
    /// <example>User</example>
    public string Role { get; set; } = "User";
}

/// <summary>Request body for POST /api/auth/login.</summary>
public class LoginRequestDto
{
    /// <example>john.doe@example.com</example>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
    public string Email { get; set; } = string.Empty;

    /// <example>SecurePass@123</example>
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}

/// <summary>Response returned after a successful login.</summary>
public class LoginResponseDto
{
    /// <summary>The JWT Bearer token to include in the Authorization header.</summary>
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
    public string Token { get; set; } = string.Empty;

    /// <summary>Token type — always "Bearer".</summary>
    /// <example>Bearer</example>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>UTC date/time when the token expires.</summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>The authenticated user's profile.</summary>
    public UserProfileDto User { get; set; } = new();
}

/// <summary>Public user profile — safe to return to clients.</summary>
public class UserProfileDto
{
    /// <example>1</example>
    public int Id { get; set; }

    /// <example>John Doe</example>
    public string FullName { get; set; } = string.Empty;

    /// <example>john.doe@example.com</example>
    public string Email { get; set; } = string.Empty;

    /// <example>User</example>
    public string Role { get; set; } = string.Empty;

    /// <summary>UTC timestamp when the account was created.</summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>Standard API response wrapper for consistent response shape.</summary>
public class ApiResponseDto<T>
{
    /// <summary>Whether the operation succeeded.</summary>
    public bool Success { get; set; }

    /// <summary>Human-readable message describing the result.</summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>The response payload (null on failure).</summary>
    public T? Data { get; set; }

    /// <summary>UTC timestamp of the response.</summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Factory helpers for clean controller code
    public static ApiResponseDto<T> Ok(T data, string message = "Success") =>
        new() { Success = true, Message = message, Data = data };

    public static ApiResponseDto<T> Fail(string message) =>
        new() { Success = false, Message = message };
}
