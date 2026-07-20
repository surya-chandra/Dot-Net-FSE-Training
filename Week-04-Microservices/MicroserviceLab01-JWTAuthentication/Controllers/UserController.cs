using System.Security.Claims;
using JWTAuthService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthService.Controllers;

// ============================================================
//  USER CONTROLLER
//  ----------------
//  Endpoints accessible to authenticated users with Role = "User".
//
//  DIFFERENCE FROM ADMIN:
//  ----------------------
//  [Authorize(Roles = "User")]  → only "User" role can access
//  [Authorize(Roles = "Admin")] → only "Admin" role can access
//  [Authorize]                  → any authenticated user
//
//  In a real system, you might use [Authorize(Roles = "Admin,User")]
//  to allow both roles to access certain endpoints.
// ============================================================

/// <summary>
/// User-level endpoints. Requires [Authorize(Roles = "User")].
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    // -------------------------------------------------------
    // GET /api/user/dashboard
    // Requires: valid JWT token with Role = "User"
    // -------------------------------------------------------

    /// <summary>
    /// User dashboard — personalised welcome message.
    /// Requires User role. Returns 403 for Admin users.
    /// </summary>
    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetDashboard()
    {
        // Read the user's name from the JWT claims
        var userName = User.FindFirstValue(JwtRegisteredClaimNames.Name)
                    ?? User.FindFirstValue(ClaimTypes.Name)
                    ?? "User";

        _logger.LogInformation("User dashboard accessed by: {User}", userName);

        var dashboardData = new
        {
            title       = "User Dashboard",
            message     = $"Welcome, {userName}! Here is your personal dashboard.",
            accessLevel = "User",
            features    = new[]
            {
                "View your profile",
                "Browse products",
                "Manage your orders",
                "Update account settings"
            },
            accessedAt = DateTime.UtcNow
        };

        return Ok(ApiResponseDto<object>.Ok(dashboardData,
            "User dashboard loaded successfully."));
    }
}

// Needed for JwtRegisteredClaimNames in this file
file static class JwtRegisteredClaimNames
{
    public const string Name = "name";
}
