namespace EFCoreLab01.Models;

/// <summary>
/// Represents a product in the Retail Inventory System.
/// This is the central entity that links Category and Supplier.
/// </summary>
public class Product
{
    /// <summary>Primary key.</summary>
    public int Id { get; set; }

    /// <summary>Product name (e.g., "Laptop Pro 15").</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Retail price of the product.</summary>
    public decimal Price { get; set; }

    /// <summary>Current stock quantity.</summary>
    public int StockQuantity { get; set; }

    // Foreign key — links this product to a Category
    public int CategoryId { get; set; }

    // Navigation property — the Category this product belongs to
    public Category? Category { get; set; }

    // Foreign key — links this product to a Supplier
    public int SupplierId { get; set; }

    // Navigation property — the Supplier who provides this product
    public Supplier? Supplier { get; set; }
}
