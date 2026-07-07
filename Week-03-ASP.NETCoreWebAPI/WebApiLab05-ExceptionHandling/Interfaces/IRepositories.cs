using WebApiLab05.Models;

namespace WebApiLab05.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task<Product?> UpdateAsync(int id, Product product);
    Task<bool> DeleteAsync(int id);
    Task<bool> CategoryExistsAsync(int categoryId);
    Task<bool> SupplierExistsAsync(int supplierId);
}

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(Product product);
    Task<Product> UpdateProductAsync(int id, Product product);
    Task DeleteProductAsync(int id);
}
