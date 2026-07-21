using EFCoreLab02.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreLab02.Data;











public class ApplicationDbContext : DbContext
{






    public DbSet<Category> Categories { get; set; }

    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<Product> Products { get; set; }







    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {




        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;" +
            "Database=RetailInventoryDb_Lab02;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True;"
        );


        optionsBuilder.EnableSensitiveDataLogging();
    }







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

            entity.HasOne(p => p.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(p => p.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Supplier)
                  .WithMany(s => s.Products)
                  .HasForeignKey(p => p.SupplierId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
