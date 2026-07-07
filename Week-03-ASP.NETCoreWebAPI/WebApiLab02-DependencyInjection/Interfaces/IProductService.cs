using WebApiLab02.Models;

namespace WebApiLab02.Interfaces;

/// <summary>
/// Defines the business logic contract for Product operations.
/// The Service layer sits between the Controller and Repository.
/// </summary>
public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(Product product);
    Task<Product?> UpdateProductAsync(int id, Product product);
    Task<bool> DeleteProductAsync(int id);
}
