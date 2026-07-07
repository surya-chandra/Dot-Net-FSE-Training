using WebApiLab03.Interfaces;
using WebApiLab03.Models;

namespace WebApiLab03.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Product>> GetAllProductsAsync() =>
        _repository.GetAllAsync();

    public Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId) =>
        _repository.GetByCategoryAsync(categoryId);

    public Task<IEnumerable<Product>> SearchProductsAsync(string? name, decimal? minPrice, decimal? maxPrice) =>
        _repository.SearchAsync(name, minPrice, maxPrice);

    public Task<Product?> GetProductByIdAsync(int id) =>
        _repository.GetByIdAsync(id);

    public async Task<Product> CreateProductAsync(Product product)
    {
        if (product.Price <= 0)
            throw new ArgumentException("Price must be greater than zero.");
        if (product.StockQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative.");

        return await _repository.CreateAsync(product);
    }

    public async Task<Product?> UpdateProductAsync(int id, Product product)
    {
        if (product.Price <= 0)
            throw new ArgumentException("Price must be greater than zero.");

        return await _repository.UpdateAsync(id, product);
    }

    public Task<bool> DeleteProductAsync(int id) =>
        _repository.DeleteAsync(id);
}
