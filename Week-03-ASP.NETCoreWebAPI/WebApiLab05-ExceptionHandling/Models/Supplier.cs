namespace WebApiLab05.Models;

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
