using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreLab02.Models;

/// <summary>
/// Represents a product category.
/// Data Annotations are used here to demonstrate an alternative
/// to Fluent API for configuring entity properties.
/// </summary>
public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    // Navigation property
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
