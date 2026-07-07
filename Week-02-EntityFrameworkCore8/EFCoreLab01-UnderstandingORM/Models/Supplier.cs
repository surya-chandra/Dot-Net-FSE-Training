namespace EFCoreLab01.Models;

/// <summary>
/// Represents a supplier who provides products to the retail store.
/// </summary>
public class Supplier
{
    /// <summary>Primary key.</summary>
    public int Id { get; set; }

    /// <summary>Company name of the supplier.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Contact email address.</summary>
    public string? Email { get; set; }

    /// <summary>Contact phone number.</summary>
    public string? Phone { get; set; }

    // Navigation property — one Supplier provides many Products
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
