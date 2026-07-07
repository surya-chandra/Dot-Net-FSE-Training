using WebApiLab02.Interfaces;
using WebApiLab02.Models;

namespace WebApiLab02.Services;

// ============================================================
//  SERVICE LAYER
//  -------------
//  The Service contains business logic and orchestrates calls
//  to one or more repositories.
//
//  It depends on IProductRepository (the interface), NOT on
//  ProductRepository (the concrete class). This is the
//  Dependency Inversion Principle in practice.
//
//  INVERSION OF CONTROL (IoC):
//  ----------------------------
//  Instead of ProductService creating its own repository:
//      var repo = new ProductRepository(context);  // ❌ tight coupling
//
//  The DI container injects it:
//      public ProductService(IProductRepository repo) { }  // ✓ loose coupling
// ============================================================

/// <summary>
/// Business logic layer for Product operations.
/// Depends on IProductRepository via constructor injection.
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    // Constructor injection — DI container provides IProductRepository
    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        // Business rule: price must be positive
        if (product.Price <= 0)
            throw new ArgumentException("Product price must be greater than zero.");

        // Business rule: stock cannot be negative
        if (product.StockQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative.");

        return await _repository.CreateAsync(product);
    }

    public async Task<Product?> UpdateProductAsync(int id, Product product)
    {
        if (product.Price <= 0)
            throw new ArgumentException("Product price must be greater than zero.");

        return await _repository.UpdateAsync(id, product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
