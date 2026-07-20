using JWTAuthService.DTOs;
using JWTAuthService.Models;

namespace JWTAuthService.Interfaces;

// ============================================================
//  INTERFACES — DEPENDENCY INVERSION PRINCIPLE
//  ---------------------------------------------
//  All layers depend on abstractions (interfaces), not on
//  concrete implementations. This enables:
//  - Unit testing with mocks
//  - Swapping implementations without changing callers
//  - Clean separation of concerns
// ============================================================

/// <summary>
/// Data access contract for User operations.
/// Only the Repository implementation touches the DbContext.
/// </summary>
public interface IUserRepository
{
    /// <summary>Finds a user by their unique email address.</summary>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>Finds a user by their primary key.</summary>
    Task<User?> GetByIdAsync(int id);

    /// <summary>Persists a new user to the database.</summary>
    Task<User> CreateAsync(User user);

    /// <summary>Returns true if a user with the given email already exists.</summary>
    Task<bool> EmailExistsAsync(string email);

    /// <summary>Returns all registered users (Admin use only).</summary>
    Task<IEnumerable<User>> GetAllAsync();
}

/// <summary>
/// Business logic contract for authentication operations.
/// </summary>
public interface IAuthService
{
    /// <summary>Registers a new user. Throws if email is already taken.</summary>
    Task<UserProfileDto> RegisterAsync(RegisterRequestDto request);

    /// <summary>
    /// Validates credentials and returns a JWT token on success.
    /// Returns null if credentials are invalid.
    /// </summary>
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);

    /// <summary>Returns the profile of the currently authenticated user.</summary>
    Task<UserProfileDto?> GetProfileAsync(int userId);
}

/// <summary>
/// Contract for JWT token operations.
/// Responsible ONLY for token generation and validation.
/// </summary>
public interface IJwtService
{
    /// <summary>Generates a signed JWT token for the given user.</summary>
    string GenerateToken(User user);

    /// <summary>
    /// Validates a token and returns the user ID claim.
    /// Returns null if the token is invalid or expired.
    /// </summary>
    int? ValidateTokenAndGetUserId(string token);

    /// <summary>Returns the UTC expiration time for a new token.</summary>
    DateTime GetExpirationTime();
}
