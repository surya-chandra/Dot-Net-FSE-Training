using System.Security.Claims;
using JWTAuthService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthService.Controllers;

















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








    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetDashboard()
    {

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

file static class JwtRegisteredClaimNames
{
    public const string Name = "name";
}
