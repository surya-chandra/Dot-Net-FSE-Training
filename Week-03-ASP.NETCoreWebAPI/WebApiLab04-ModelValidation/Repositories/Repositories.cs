using Microsoft.EntityFrameworkCore;
using WebApiLab04.Data;
using WebApiLab04.Interfaces;
using WebApiLab04.Models;

namespace WebApiLab04.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Product>> GetAllAsync() =>
        await _context.Products.Include(p => p.Category).Include(p => p.Supplier).ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) =>
        await _context.Products.Include(p => p.Category).Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateAsync(int id, Product product)
    {
        var existing = await _context.Products.FindAsync(id);
        if (existing is null) return null;

        existing.Name          = product.Name;
        existing.Price         = product.Price;
        existing.StockQuantity = product.StockQuantity;
        existing.CategoryId    = product.CategoryId;
        existing.SupplierId    = product.SupplierId;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null) return false;
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    public Task<bool> CategoryExistsAsync(int categoryId) =>
        _context.Categories.AnyAsync(c => c.Id == categoryId);

    public Task<bool> SupplierExistsAsync(int supplierId) =>
        _context.Suppliers.AnyAsync(s => s.Id == supplierId);
}

public class SupplierRepository : ISupplierRepository
{
    private readonly ApplicationDbContext _context;

    public SupplierRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Supplier>> GetAllAsync() =>
        await _context.Suppliers.ToListAsync();

    public async Task<Supplier?> GetByIdAsync(int id) =>
        await _context.Suppliers.FindAsync(id);

    public async Task<Supplier> CreateAsync(Supplier supplier)
    {
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        return supplier;
    }
}

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Category>> GetAllAsync() =>
        await _context.Categories.ToListAsync();

    public async Task<Category?> GetByIdAsync(int id) =>
        await _context.Categories.FindAsync(id);

    public async Task<Category> CreateAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }
}
