using EFCoreLab02.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreLab02.Data;

/// <summary>
/// ApplicationDbContext — the core of EF Core's database interaction.
///
/// This class:
///   1. Inherits DbContext to gain all EF Core capabilities
///   2. Exposes DbSet<T> properties — one per database table
///   3. Configures the SQL Server connection in OnConfiguring()
///   4. Applies Fluent API rules in OnModelCreating()
///
/// Lifetime: Create → Use → Dispose  (use 'using' statement)
/// </summary>
public class ApplicationDbContext : DbContext
{
    // -------------------------------------------------------
    // DbSet<T> Properties
    // Each DbSet<T> represents a table in the database.
    // EF Core uses these to build and execute SQL queries.
    // -------------------------------------------------------

    /// <summary>Maps to the [Categories] table.</summary>
    public DbSet<Category> Categories { get; set; }

    /// <summary>Maps to the [Suppliers] table.</summary>
    public DbSet<Supplier> Suppliers { get; set; }

    /// <summary>Maps to the [Products] table.</summary>
    public DbSet<Product> Products { get; set; }

    // -------------------------------------------------------
    // Database Connection Configuration
    // -------------------------------------------------------

    /// <summary>
    /// Called by EF Core to configure the database provider.
    /// UseSqlServer() registers the SQL Server provider.
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Connection string components:
        //   Server   — SQL Server instance name
        //   Database — name of the database to create/use
        //   Trusted_Connection — use Windows Authentication
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;" +
            "Database=RetailInventoryDb_Lab02;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True;"
        );

        // Enable sensitive data logging during development
        // (shows parameter values in logs — disable in production)
        optionsBuilder.EnableSensitiveDataLogging();
    }

    // -------------------------------------------------------
    // Fluent API Model Configuration
    // -------------------------------------------------------

    /// <summary>
    /// Called by EF Core when building the model.
    /// Fluent API here overrides or supplements Data Annotations.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureCategory(modelBuilder);
        ConfigureSupplier(modelBuilder);
        ConfigureProduct(modelBuilder);
    }

    private static void ConfigureCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(c => c.Description)
                  .HasMaxLength(500);

            // Unique constraint — no two categories with the same name
            entity.HasIndex(c => c.Name).IsUnique();
        });
    }

    private static void ConfigureSupplier(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Suppliers");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name)
                  .IsRequired()
                  .HasMaxLength(150);
            entity.Property(s => s.Email).HasMaxLength(200);
            entity.Property(s => s.Phone).HasMaxLength(20);
        });
    }

    private static void ConfigureProduct(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(200);
            entity.Property(p => p.Price)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();
            entity.Property(p => p.StockQuantity)
                  .HasDefaultValue(0);

            // Relationship: Product → Category (many-to-one)
            entity.HasOne(p => p.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(p => p.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relationship: Product → Supplier (many-to-one)
            entity.HasOne(p => p.Supplier)
                  .WithMany(s => s.Products)
                  .HasForeignKey(p => p.SupplierId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
