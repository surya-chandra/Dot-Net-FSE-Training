using WebApiLab04.Interfaces;
using WebApiLab04.Models;

namespace WebApiLab04.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository) => _repository = repository;

    public Task<IEnumerable<Product>> GetAllProductsAsync() => _repository.GetAllAsync();

    public Task<Product?> GetProductByIdAsync(int id) => _repository.GetByIdAsync(id);

    public async Task<Product> CreateProductAsync(Product product)
    {
        if (!await _repository.CategoryExistsAsync(product.CategoryId))
            throw new ArgumentException($"CategoryId={product.CategoryId} does not exist.");

        if (!await _repository.SupplierExistsAsync(product.SupplierId))
            throw new ArgumentException($"SupplierId={product.SupplierId} does not exist.");

        return await _repository.CreateAsync(product);
    }

    public async Task<Product?> UpdateProductAsync(int id, Product product)
    {
        if (!await _repository.CategoryExistsAsync(product.CategoryId))
            throw new ArgumentException($"CategoryId={product.CategoryId} does not exist.");

        if (!await _repository.SupplierExistsAsync(product.SupplierId))
            throw new ArgumentException($"SupplierId={product.SupplierId} does not exist.");

        return await _repository.UpdateAsync(id, product);
    }

    public Task<bool> DeleteProductAsync(int id) => _repository.DeleteAsync(id);
}
