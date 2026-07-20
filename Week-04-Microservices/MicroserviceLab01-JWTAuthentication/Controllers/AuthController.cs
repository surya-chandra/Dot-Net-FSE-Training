using System.Security.Claims;
using JWTAuthService.DTOs;
using JWTAuthService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthService.Controllers;

// ============================================================
//  AUTH CONTROLLER
//  ----------------
//  Handles authentication endpoints:
//    POST /api/auth/register  — public (no token required)
//    POST /api/auth/login     — public (no token required)
//    GET  /api/auth/profile   — protected (valid token required)
//
//  [Authorize]  — requires a valid JWT Bearer token
//  [AllowAnonymous] — explicitly allows unauthenticated access
//
//  HOW [Authorize] WORKS:
//  ----------------------
//  1. Client sends: Authorization: Bearer <token>
//  2. JwtBearer middleware validates the token signature,
//     issuer, audience, and expiry
//  3. If valid, the user's claims are loaded into HttpContext.User
//  4. [Authorize] checks that HttpContext.User.Identity.IsAuthenticated
//  5. If not authenticated → 401 Unauthorized (automatic)
// ============================================================

/// <summary>
/// Authentication endpoints — Register, Login, and Profile.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger      = logger;
    }

    // -------------------------------------------------------
    // POST /api/auth/register
    // Public endpoint — no authentication required
    // -------------------------------------------------------

    /// <summary>
    /// Registers a new user account.
    /// Passwords are hashed using PBKDF2 before storage.
    /// </summary>
    /// <param name="request">Registration details.</param>
    /// <returns>The created user profile.</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponseDto<UserProfileDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        // [ApiController] validates the DTO automatically.
        // If we reach here, all Data Annotations have passed.
        _logger.LogInformation("Register request received for: {Email}", request.Email);

        var profile = await _authService.RegisterAsync(request);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponseDto<UserProfileDto>.Ok(profile, "User registered successfully."));
    }

    // -------------------------------------------------------
    // POST /api/auth/login
    // Public endpoint — returns JWT token on success
    // -------------------------------------------------------

    /// <summary>
    /// Authenticates a user and returns a JWT Bearer token.
    /// Include the token in the Authorization header for protected endpoints:
    /// Authorization: Bearer &lt;token&gt;
    /// </summary>
    /// <param name="request">Login credentials.</param>
    /// <returns>JWT token and user profile on success.</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponseDto<LoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        _logger.LogInformation("Login request received for: {Email}", request.Email);

        var result = await _authService.LoginAsync(request);

        if (result is null)
        {
            _logger.LogWarning("Login failed for: {Email}", request.Email);
            return Unauthorized(
                ApiResponseDto<object>.Fail("Invalid email or password."));
        }

        return Ok(ApiResponseDto<LoginResponseDto>.Ok(result, "Login successful."));
    }

    // -------------------------------------------------------
    // GET /api/auth/profile
    // Protected — requires valid JWT token
    // -------------------------------------------------------

    /// <summary>
    /// Returns the profile of the currently authenticated user.
    /// Requires a valid JWT Bearer token in the Authorization header.
    /// </summary>
    /// <returns>The authenticated user's profile.</returns>
    [HttpGet("profile")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponseDto<UserProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile()
    {
        // Extract the user ID from the JWT claims
        // ClaimTypes.NameIdentifier maps to the "sub" claim we set in JwtService
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim is null || !int.TryParse(userIdClaim, out int userId))
            return Unauthorized(ApiResponseDto<object>.Fail("Invalid token claims."));

        var profile = await _authService.GetProfileAsync(userId);

        if (profile is null)
            return NotFound(ApiResponseDto<object>.Fail("User not found."));

        return Ok(ApiResponseDto<UserProfileDto>.Ok(profile));
    }
}
