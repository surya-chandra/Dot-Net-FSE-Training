using JWTAuthService.DTOs;
using JWTAuthService.Helpers;
using JWTAuthService.Interfaces;
using JWTAuthService.Models;

namespace JWTAuthService.Services;

















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

    public async Task<UserProfileDto> RegisterAsync(RegisterRequestDto request)
    {
        _logger.LogInformation("Registration attempt for email: {Email}", request.Email);

        if (await _userRepository.EmailExistsAsync(request.Email))
        {
            _logger.LogWarning("Registration failed — email already exists: {Email}", request.Email);
            throw new InvalidOperationException($"An account with email '{request.Email}' already exists.");
        }

        var allowedRoles = new[] { "Admin", "User" };
        var role = allowedRoles.Contains(request.Role, StringComparer.OrdinalIgnoreCase)
            ? request.Role
            : "User";

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

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        _logger.LogInformation("Login attempt for email: {Email}", request.Email);

        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
        {

            _logger.LogWarning("Login failed — email not found: {Email}", request.Email);
            return null;
        }

        if (!PasswordHelper.VerifyPassword(user.PasswordHash, request.Password))
        {
            _logger.LogWarning("Login failed — invalid password for email: {Email}", request.Email);
            return null;
        }

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

    public async Task<UserProfileDto?> GetProfileAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return user is null ? null : MapToProfileDto(user);
    }



    private static UserProfileDto MapToProfileDto(User user) => new()
    {
        Id        = user.Id,
        FullName  = user.FullName,
        Email     = user.Email,
        Role      = user.Role,
        CreatedAt = user.CreatedAt
    };
}
