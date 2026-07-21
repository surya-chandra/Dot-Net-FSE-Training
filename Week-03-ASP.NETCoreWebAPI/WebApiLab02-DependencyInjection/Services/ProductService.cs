using WebApiLab02.Interfaces;
using WebApiLab02.Models;

namespace WebApiLab02.Services;






















public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

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

        if (product.Price <= 0)
            throw new ArgumentException("Product price must be greater than zero.");

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
