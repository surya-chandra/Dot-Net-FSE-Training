using WebApiLab02.Models;

namespace WebApiLab02.Interfaces;

// ============================================================
//  DEPENDENCY INVERSION PRINCIPLE
//  --------------------------------
//  High-level modules (Service, Controller) should NOT depend
//  on low-level modules (Repository). Both should depend on
//  abstractions (Interfaces).
//
//  This interface defines the CONTRACT for product data access.
//  The concrete implementation (ProductRepository) can be
//  swapped without changing the Service or Controller.
// ============================================================

/// <summary>
/// Defines the data access contract for Product operations.
/// </summary>
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task<Product?> UpdateAsync(int id, Product product);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
