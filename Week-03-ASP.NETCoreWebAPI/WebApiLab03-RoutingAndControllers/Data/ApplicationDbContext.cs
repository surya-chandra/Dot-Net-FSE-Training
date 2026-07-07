using Microsoft.EntityFrameworkCore;
using WebApiLab03.Models;

namespace WebApiLab03.Data;

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
            new Category { Id = 3, Name = "Home & Garden", Description = "Furniture, tools, garden supplies" },
            new Category { Id = 4, Name = "Sports",        Description = "Sports equipment and outdoor gear" }
        );

        modelBuilder.Entity<Supplier>().HasData(
            new Supplier { Id = 1, Name = "TechSupply Co.",  Email = "orders@techsupply.com",  Phone = "+1-800-555-0100" },
            new Supplier { Id = 2, Name = "FashionHub Ltd.", Email = "sales@fashionhub.com",   Phone = "+1-800-555-0200" },
            new Supplier { Id = 3, Name = "HomeGoods Inc.",  Email = "contact@homegoods.com",  Phone = "+1-800-555-0300" },
            new Supplier { Id = 4, Name = "SportZone Corp.", Email = "info@sportzone.com",     Phone = "+1-800-555-0400" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop Pro 15",       Price = 1299.99m, StockQuantity = 50,  CategoryId = 1, SupplierId = 1 },
            new Product { Id = 2, Name = "Wireless Mouse",      Price =   29.99m, StockQuantity = 200, CategoryId = 1, SupplierId = 1 },
            new Product { Id = 3, Name = "Mechanical Keyboard", Price =   89.99m, StockQuantity = 150, CategoryId = 1, SupplierId = 1 },
            new Product { Id = 4, Name = "4K Monitor 27\"",     Price =  449.99m, StockQuantity = 30,  CategoryId = 1, SupplierId = 1 },
            new Product { Id = 5, Name = "Running Shoes",       Price =   79.99m, StockQuantity = 100, CategoryId = 2, SupplierId = 2 },
            new Product { Id = 6, Name = "Denim Jacket",        Price =   59.99m, StockQuantity = 75,  CategoryId = 2, SupplierId = 2 },
            new Product { Id = 7, Name = "Garden Hose 50ft",    Price =   34.99m, StockQuantity = 60,  CategoryId = 3, SupplierId = 3 },
            new Product { Id = 8, Name = "Power Drill",         Price =  119.99m, StockQuantity = 40,  CategoryId = 3, SupplierId = 3 },
            new Product { Id = 9, Name = "Yoga Mat",            Price =   24.99m, StockQuantity = 120, CategoryId = 4, SupplierId = 4 },
            new Product { Id = 10, Name = "Dumbbells Set 20kg", Price =   69.99m, StockQuantity = 55,  CategoryId = 4, SupplierId = 4 }
        );
    }
}
