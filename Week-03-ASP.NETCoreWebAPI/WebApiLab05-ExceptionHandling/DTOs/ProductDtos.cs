using System.ComponentModel.DataAnnotations;

namespace WebApiLab05.DTOs;

public class CreateProductDto
{
    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 200 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 999999.99, ErrorMessage = "Price must be between $0.01 and $999,999.99.")]
    public decimal Price { get; set; }

    [Range(0, 100000, ErrorMessage = "Stock quantity must be between 0 and 100,000.")]
    public int StockQuantity { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a positive integer.")]
    public int CategoryId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "SupplierId must be a positive integer.")]
    public int SupplierId { get; set; }
}

public class UpdateProductDto
{
    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 200 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 999999.99, ErrorMessage = "Price must be between $0.01 and $999,999.99.")]
    public decimal Price { get; set; }

    [Range(0, 100000, ErrorMessage = "Stock quantity must be between 0 and 100,000.")]
    public int StockQuantity { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a positive integer.")]
    public int CategoryId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "SupplierId must be a positive integer.")]
    public int SupplierId { get; set; }
}

public class ProductResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
}
