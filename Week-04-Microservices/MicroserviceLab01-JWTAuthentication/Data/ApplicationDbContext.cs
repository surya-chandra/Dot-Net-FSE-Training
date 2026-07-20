using JWTAuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthService.Data;

/// <summary>
/// EF Core DbContext for the JWT Authentication Microservice.
///
/// In a microservice architecture, each service owns its own database.
/// This context manages only the Users table — no shared schema.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    /// <summary>Maps to the [Users] table.</summary>
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.FullName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(u => u.Email)
                  .IsRequired()
                  .HasMaxLength(200);

            // Unique constraint — no two users with the same email
            entity.HasIndex(u => u.Email)
                  .IsUnique()
                  .HasDatabaseName("IX_Users_Email");

            entity.Property(u => u.PasswordHash)
                  .IsRequired()
                  .HasMaxLength(500);

            entity.Property(u => u.Role)
                  .IsRequired()
                  .HasMaxLength(20)
                  .HasDefaultValue("User");

            entity.Property(u => u.CreatedAt)
                  .HasDefaultValueSql("GETUTCDATE()");
        });
    }
}
