namespace EFCoreLab01.Models;

/// <summary>
/// Represents a product category in the Retail Inventory System.
/// </summary>
public class Category
{
    /// <summary>Primary key — EF Core recognises the "Id" convention automatically.</summary>
    public int Id { get; set; }

    /// <summary>Display name of the category (e.g., Electronics, Clothing).</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Optional description of the category.</summary>
    public string? Description { get; set; }

    // Navigation property — one Category has many Products
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
