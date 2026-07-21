using JWTAuthService.DTOs;
using JWTAuthService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthService.Controllers;



















[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
[Produces("application/json")]
public class AdminController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IUserRepository userRepository, ILogger<AdminController> logger)
    {
        _userRepository = userRepository;
        _logger         = logger;
    }








    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetDashboard()
    {
        _logger.LogInformation("Admin dashboard accessed by: {User}",
            User.Identity?.Name ?? "Unknown");

        var dashboardData = new
        {
            title       = "Admin Dashboard",
            message     = "Welcome, Administrator! You have full system access.",
            accessLevel = "Admin",
            features    = new[]
            {
                "View all users",
                "Manage roles",
                "View system statistics",
                "Access audit logs"
            },
            accessedAt = DateTime.UtcNow
        };

        return Ok(ApiResponseDto<object>.Ok(dashboardData,
            "Admin dashboard loaded successfully."));
    }








    [HttpGet("users")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<UserProfileDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllUsers()
    {
        _logger.LogInformation("Admin requested all users list.");

        var users = await _userRepository.GetAllAsync();

        var profiles = users.Select(u => new UserProfileDto
        {
            Id        = u.Id,
            FullName  = u.FullName,
            Email     = u.Email,
            Role      = u.Role,
            CreatedAt = u.CreatedAt
        });

        return Ok(ApiResponseDto<IEnumerable<UserProfileDto>>.Ok(profiles,
            $"{profiles.Count()} user(s) found."));
    }
}
