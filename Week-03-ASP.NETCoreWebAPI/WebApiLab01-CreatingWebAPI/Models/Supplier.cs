namespace WebApiLab01.Models;

/// <summary>Represents a product supplier.</summary>
public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
