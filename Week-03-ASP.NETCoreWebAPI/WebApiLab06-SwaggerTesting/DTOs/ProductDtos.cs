using System.ComponentModel.DataAnnotations;

namespace WebApiLab06.DTOs;

/// <summary>Request body for creating a new product.</summary>
public class CreateProductDto
{
    /// <example>Laptop Pro 15</example>
    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(200, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    /// <example>1299.99</example>
    [Required]
    [Range(0.01, 999999.99)]
    public decimal Price { get; set; }

    /// <example>50</example>
    [Range(0, 100000)]
    public int StockQuantity { get; set; }

    /// <example>1</example>
    [Required]
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    /// <example>1</example>
    [Required]
    [Range(1, int.MaxValue)]
    public int SupplierId { get; set; }
}

/// <summary>Request body for updating an existing product.</summary>
public class UpdateProductDto
{
    /// <example>Laptop Pro 15 (Updated)</example>
    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(200, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    /// <example>1199.99</example>
    [Required]
    [Range(0.01, 999999.99)]
    public decimal Price { get; set; }

    /// <example>45</example>
    [Range(0, 100000)]
    public int StockQuantity { get; set; }

    /// <example>1</example>
    [Required]
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    /// <example>1</example>
    [Required]
    [Range(1, int.MaxValue)]
    public int SupplierId { get; set; }
}

/// <summary>Product data returned by the API.</summary>
public class ProductResponseDto
{
    /// <example>1</example>
    public int Id { get; set; }

    /// <example>Laptop Pro 15</example>
    public string Name { get; set; } = string.Empty;

    /// <example>1299.99</example>
    public decimal Price { get; set; }

    /// <example>50</example>
    public int StockQuantity { get; set; }

    /// <example>Electronics</example>
    public string CategoryName { get; set; } = string.Empty;

    /// <example>TechSupply Co.</example>
    public string SupplierName { get; set; } = string.Empty;
}
