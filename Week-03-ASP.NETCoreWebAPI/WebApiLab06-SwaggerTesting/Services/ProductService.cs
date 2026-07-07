using WebApiLab06.Exceptions;
using WebApiLab06.Interfaces;
using WebApiLab06.Models;

namespace WebApiLab06.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository repository, ILogger<ProductService> logger)
    {
        _repository = repository;
        _logger     = logger;
    }

    public Task<IEnumerable<Product>> GetAllProductsAsync() => _repository.GetAllAsync();

    public Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId) =>
        _repository.GetByCategoryAsync(categoryId);

    public Task<IEnumerable<Product>> SearchProductsAsync(string? name, decimal? minPrice, decimal? maxPrice) =>
        _repository.SearchAsync(name, minPrice, maxPrice);

    public async Task<Product> GetProductByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is null) throw new NotFoundException("Product", id);
        return product;
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        if (product.Price <= 0)
            throw new ValidationException("Price must be greater than zero.");
        if (product.StockQuantity < 0)
            throw new ValidationException("Stock quantity cannot be negative.");
        if (!await _repository.CategoryExistsAsync(product.CategoryId))
            throw new ValidationException($"CategoryId={product.CategoryId} does not exist.");
        if (!await _repository.SupplierExistsAsync(product.SupplierId))
            throw new ValidationException($"SupplierId={product.SupplierId} does not exist.");

        var created = await _repository.CreateAsync(product);
        _logger.LogInformation("Product created: Id={Id}, Name={Name}", created.Id, created.Name);
        return created;
    }

    public async Task<Product> UpdateProductAsync(int id, Product product)
    {
        if (product.Price <= 0)
            throw new ValidationException("Price must be greater than zero.");
        if (!await _repository.CategoryExistsAsync(product.CategoryId))
            throw new ValidationException($"CategoryId={product.CategoryId} does not exist.");
        if (!await _repository.SupplierExistsAsync(product.SupplierId))
            throw new ValidationException($"SupplierId={product.SupplierId} does not exist.");

        var updated = await _repository.UpdateAsync(id, product);
        if (updated is null) throw new NotFoundException("Product", id);

        _logger.LogInformation("Product updated: Id={Id}", id);
        return updated;
    }

    public async Task DeleteProductAsync(int id)
    {
        var deleted = await _repository.DeleteAsync(id);
        if (!deleted) throw new NotFoundException("Product", id);
        _logger.LogInformation("Product deleted: Id={Id}", id);
    }
}
