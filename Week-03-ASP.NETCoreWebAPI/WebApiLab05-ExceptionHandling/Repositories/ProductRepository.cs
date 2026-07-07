using Microsoft.EntityFrameworkCore;
using WebApiLab05.Data;
using WebApiLab05.Interfaces;
using WebApiLab05.Models;

namespace WebApiLab05.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Product>> GetAllAsync() =>
        await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) =>
        await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
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
