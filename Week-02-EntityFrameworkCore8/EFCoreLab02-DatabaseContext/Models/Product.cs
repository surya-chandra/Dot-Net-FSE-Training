using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreLab02.Models;

/// <summary>
/// Represents a product in the Retail Inventory System.
/// </summary>
public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    // Column attribute specifies the SQL column type explicitly
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    // Foreign key to Category
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    // Foreign key to Supplier
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
}
