using EFCoreLab03.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreLab03.Data;

/// <summary>
/// DbContext for Lab 03 — Migrations demo.
/// EnsureCreated() vs Migrate() is demonstrated in Program.cs.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;" +
            "Database=RetailInventoryDb_Lab03;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True;"
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Name).IsRequired().HasMaxLength(100);
            e.Property(c => c.Description).HasMaxLength(500);
            e.HasIndex(c => c.Name).IsUnique();
        });

        modelBuilder.Entity<Supplier>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Name).IsRequired().HasMaxLength(150);
            e.Property(s => s.Email).HasMaxLength(200);
            e.Property(s => s.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).IsRequired().HasMaxLength(200);
            e.Property(p => p.Price).HasColumnType("decimal(18,2)");

            e.HasOne(p => p.Category)
             .WithMany(c => c.Products)
             .HasForeignKey(p => p.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(p => p.Supplier)
             .WithMany(s => s.Products)
             .HasForeignKey(p => p.SupplierId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
