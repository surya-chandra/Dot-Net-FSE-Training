using WebApiLab05.Exceptions;
using WebApiLab05.Interfaces;
using WebApiLab05.Models;

namespace WebApiLab05.Services;

// ============================================================
//  EXCEPTION HANDLING BEST PRACTICES
//  -----------------------------------
//  The Service layer throws typed custom exceptions.
//  The GlobalExceptionMiddleware catches them and maps each
//  to the correct HTTP status code.
//
//  This keeps controllers clean — no try-catch needed there.
//  Controllers simply call the service and return the result.
// ============================================================

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository repository, ILogger<ProductService> logger)
    {
        _repository = repository;
        _logger     = logger;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        _logger.LogInformation("Retrieving all products.");
        return await _repository.GetAllAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving product with Id={Id}.", id);

        var product = await _repository.GetByIdAsync(id);

        // Throw NotFoundException — middleware maps this to 404
        if (product is null)
            throw new NotFoundException("Product", id);

        return product;
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        _logger.LogInformation("Creating product: {Name}.", product.Name);

        // Business rule validation — throws ValidationException → 400
        if (product.Price <= 0)
            throw new ValidationException("Price must be greater than zero.");

        if (product.StockQuantity < 0)
            throw new ValidationException("Stock quantity cannot be negative.");

        if (!await _repository.CategoryExistsAsync(product.CategoryId))
            throw new ValidationException($"CategoryId={product.CategoryId} does not exist.");

        if (!await _repository.SupplierExistsAsync(product.SupplierId))
            throw new ValidationException($"SupplierId={product.SupplierId} does not exist.");

        var created = await _repository.CreateAsync(product);
        _logger.LogInformation("Product created with Id={Id}.", created.Id);
        return created;
    }

    public async Task<Product> UpdateProductAsync(int id, Product product)
    {
        _logger.LogInformation("Updating product with Id={Id}.", id);

        if (product.Price <= 0)
            throw new ValidationException("Price must be greater than zero.");

        if (!await _repository.CategoryExistsAsync(product.CategoryId))
            throw new ValidationException($"CategoryId={product.CategoryId} does not exist.");

        if (!await _repository.SupplierExistsAsync(product.SupplierId))
            throw new ValidationException($"SupplierId={product.SupplierId} does not exist.");

        var updated = await _repository.UpdateAsync(id, product);

        if (updated is null)
            throw new NotFoundException("Product", id);

        _logger.LogInformation("Product Id={Id} updated successfully.", id);
        return updated;
    }

    public async Task DeleteProductAsync(int id)
    {
        _logger.LogInformation("Deleting product with Id={Id}.", id);

        var deleted = await _repository.DeleteAsync(id);

        if (!deleted)
            throw new NotFoundException("Product", id);

        _logger.LogInformation("Product Id={Id} deleted successfully.", id);
    }
}
