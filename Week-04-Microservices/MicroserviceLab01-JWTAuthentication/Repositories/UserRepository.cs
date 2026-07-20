using JWTAuthService.Data;
using JWTAuthService.Interfaces;
using JWTAuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthService.Repositories;

// ============================================================
//  REPOSITORY PATTERN
//  -------------------
//  The Repository is the ONLY layer that interacts with EF Core.
//  Controllers and Services never access DbContext directly.
//
//  This ensures:
//  - Data access logic is centralised
//  - Services are testable (mock the repository)
//  - Switching from SQL Server to another DB only requires
//    changing the repository, not the service or controller
// ============================================================

/// <summary>
/// EF Core implementation of IUserRepository.
/// Handles all database operations for the Users table.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger  = logger;
    }

    /// <inheritdoc/>
    public async Task<User?> GetByEmailAsync(string email)
    {
        _logger.LogDebug("Querying user by email: {Email}", email);
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    /// <inheritdoc/>
    public async Task<User?> GetByIdAsync(int id)
    {
        _logger.LogDebug("Querying user by Id: {Id}", id);
        return await _context.Users.FindAsync(id);
    }

    /// <inheritdoc/>
    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        _logger.LogInformation("User created: Id={Id}, Email={Email}", user.Id, user.Email);
        return user;
    }

    /// <inheritdoc/>
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .OrderBy(u => u.CreatedAt)
            .ToListAsync();
    }
}
