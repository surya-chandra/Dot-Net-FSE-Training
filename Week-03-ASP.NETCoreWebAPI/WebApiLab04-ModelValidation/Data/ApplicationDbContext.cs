using Microsoft.EntityFrameworkCore;
using WebApiLab04.Models;

namespace WebApiLab04.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Name).IsRequired().HasMaxLength(100);
            e.Property(c => c.Description).HasMaxLength(500);
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

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics",   Description = "Electronic gadgets and devices" },
            new Category { Id = 2, Name = "Clothing",      Description = "Apparel and accessories" },
            new Category { Id = 3, Name = "Sports",        Description = "Sports equipment" }
        );

        modelBuilder.Entity<Supplier>().HasData(
            new Supplier { Id = 1, Name = "TechSupply Co.",  Email = "orders@techsupply.com",  Phone = "+1-800-555-0100" },
            new Supplier { Id = 2, Name = "FashionHub Ltd.", Email = "sales@fashionhub.com",   Phone = "+1-800-555-0200" },
            new Supplier { Id = 3, Name = "SportZone Corp.", Email = "info@sportzone.com",     Phone = "+1-800-555-0400" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop Pro 15",       Price = 1299.99m, StockQuantity = 50,  CategoryId = 1, SupplierId = 1 },
            new Product { Id = 2, Name = "Wireless Mouse",      Price =   29.99m, StockQuantity = 200, CategoryId = 1, SupplierId = 1 },
            new Product { Id = 3, Name = "Running Shoes",       Price =   79.99m, StockQuantity = 100, CategoryId = 2, SupplierId = 2 },
            new Product { Id = 4, Name = "Yoga Mat",            Price =   24.99m, StockQuantity = 120, CategoryId = 3, SupplierId = 3 }
        );
    }
}
