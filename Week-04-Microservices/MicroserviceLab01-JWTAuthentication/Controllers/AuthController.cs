using System.Security.Claims;
using JWTAuthService.DTOs;
using JWTAuthService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthService.Controllers;























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










    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponseDto<UserProfileDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {


        _logger.LogInformation("Register request received for: {Email}", request.Email);

        var profile = await _authService.RegisterAsync(request);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponseDto<UserProfileDto>.Ok(profile, "User registered successfully."));
    }











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









    [HttpGet("profile")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponseDto<UserProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile()
    {


        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim is null || !int.TryParse(userIdClaim, out int userId))
            return Unauthorized(ApiResponseDto<object>.Fail("Invalid token claims."));

        var profile = await _authService.GetProfileAsync(userId);

        if (profile is null)
            return NotFound(ApiResponseDto<object>.Fail("User not found."));

        return Ok(ApiResponseDto<UserProfileDto>.Ok(profile));
    }
}
