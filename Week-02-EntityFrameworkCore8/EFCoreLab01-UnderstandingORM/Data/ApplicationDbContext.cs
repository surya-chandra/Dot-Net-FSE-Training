using EFCoreLab01.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreLab01.Data;

/// <summary>
/// ApplicationDbContext is the bridge between your C# entity classes
/// and the SQL Server database. It inherits from EF Core's DbContext.
///
/// Responsibilities:
///   - Holds DbSet properties (one per entity = one per table)
///   - Configures the database connection
///   - Applies any Fluent API configuration
/// </summary>
public class ApplicationDbContext : DbContext
{
    // DbSet<T> maps to a database table.
    // EF Core uses these to generate SQL queries.
    public DbSet<Category> Categories { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// Configures the database provider and connection string.
    /// In a real application this would come from appsettings.json.
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // UseSqlServer tells EF Core to target SQL Server.
        // Replace the connection string to match your environment.
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;Database=RetailInventoryDb_Lab01;Trusted_Connection=True;"
        );
    }

    /// <summary>
    /// Fluent API configuration — fine-tunes table/column behaviour
    /// beyond what EF Core conventions can infer automatically.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Category entity
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Description).HasMaxLength(500);
        });

        // Configure Supplier entity
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(150);
            entity.Property(s => s.Email).HasMaxLength(200);
            entity.Property(s => s.Phone).HasMaxLength(20);
        });

        // Configure Product entity and its relationships
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Price).HasColumnType("decimal(18,2)");

            // Product → Category  (many-to-one)
            entity.HasOne(p => p.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(p => p.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Product → Supplier  (many-to-one)
            entity.HasOne(p => p.Supplier)
                  .WithMany(s => s.Products)
                  .HasForeignKey(p => p.SupplierId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
