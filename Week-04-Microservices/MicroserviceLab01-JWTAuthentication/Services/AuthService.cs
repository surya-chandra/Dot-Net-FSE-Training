using JWTAuthService.DTOs;
using JWTAuthService.Helpers;
using JWTAuthService.Interfaces;
using JWTAuthService.Models;

namespace JWTAuthService.Services;

// ============================================================
//  AUTH SERVICE — BUSINESS LOGIC LAYER
//  -------------------------------------
//  The AuthService orchestrates:
//    - User registration (hash password, persist user)
//    - User login (verify credentials, generate token)
//    - Profile retrieval
//
//  It depends on:
//    - IUserRepository  (data access)
//    - IJwtService      (token generation)
//
//  It does NOT know about HTTP, controllers, or EF Core directly.
// ============================================================

/// <summary>
/// Implements authentication business logic for the JWT Auth Microservice.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService     _jwtService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        IJwtService jwtService,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _jwtService     = jwtService;
        _logger         = logger;
    }

    /// <inheritdoc/>
    public async Task<UserProfileDto> RegisterAsync(RegisterRequestDto request)
    {
        _logger.LogInformation("Registration attempt for email: {Email}", request.Email);

        // Business rule: email must be unique
        if (await _userRepository.EmailExistsAsync(request.Email))
        {
            _logger.LogWarning("Registration failed — email already exists: {Email}", request.Email);
            throw new InvalidOperationException($"An account with email '{request.Email}' already exists.");
        }

        // Validate role — only "Admin" and "User" are accepted
        var allowedRoles = new[] { "Admin", "User" };
        var role = allowedRoles.Contains(request.Role, StringComparer.OrdinalIgnoreCase)
            ? request.Role
            : "User";

        // Hash the password — NEVER store plain text
        var passwordHash = PasswordHelper.HashPassword(request.Password);

        var user = new User
        {
            FullName     = request.FullName.Trim(),
            Email        = request.Email.Trim().ToLower(),
            PasswordHash = passwordHash,
            Role         = role,
            CreatedAt    = DateTime.UtcNow
        };

        var created = await _userRepository.CreateAsync(user);
        _logger.LogInformation("User registered successfully: Id={Id}, Role={Role}",
            created.Id, created.Role);

        return MapToProfileDto(created);
    }

    /// <inheritdoc/>
    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        _logger.LogInformation("Login attempt for email: {Email}", request.Email);

        // Look up the user by email
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
        {
            // Use a generic message — do not reveal whether email exists
            _logger.LogWarning("Login failed — email not found: {Email}", request.Email);
            return null;
        }

        // Verify the password against the stored hash
        if (!PasswordHelper.VerifyPassword(user.PasswordHash, request.Password))
        {
            _logger.LogWarning("Login failed — invalid password for email: {Email}", request.Email);
            return null;
        }

        // Generate JWT token
        var token     = _jwtService.GenerateToken(user);
        var expiresAt = _jwtService.GetExpirationTime();

        _logger.LogInformation("Login successful: Id={Id}, Role={Role}", user.Id, user.Role);

        return new LoginResponseDto
        {
            Token     = token,
            TokenType = "Bearer",
            ExpiresAt = expiresAt,
            User      = MapToProfileDto(user)
        };
    }

    /// <inheritdoc/>
    public async Task<UserProfileDto?> GetProfileAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return user is null ? null : MapToProfileDto(user);
    }

    // -------------------------------------------------------
    // Private helper — maps User entity to safe profile DTO
    // -------------------------------------------------------
    private static UserProfileDto MapToProfileDto(User user) => new()
    {
        Id        = user.Id,
        FullName  = user.FullName,
        Email     = user.Email,
        Role      = user.Role,
        CreatedAt = user.CreatedAt
    };
}
