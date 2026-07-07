using System.ComponentModel.DataAnnotations;

namespace WebApiLab04.DTOs;

// ============================================================
//  MODEL VALIDATION WITH DATA ANNOTATIONS
//  ----------------------------------------
//  Data Annotations are attributes applied to DTO properties.
//  ASP.NET Core's [ApiController] automatically validates the
//  model and returns 400 Bad Request if validation fails —
//  no manual ModelState.IsValid check needed.
//
//  COMMON ANNOTATIONS:
//  -------------------
//  [Required]              — field must be present and non-empty
//  [StringLength(max)]     — max (and optional min) string length
//  [Range(min, max)]       — numeric value within a range
//  [EmailAddress]          — must be a valid email format
//  [Phone]                 — must be a valid phone format
//  [MinLength(n)]          — minimum collection/string length
//  [MaxLength(n)]          — maximum collection/string length
//  [RegularExpression]     — must match a regex pattern
//  [Url]                   — must be a valid URL
// ============================================================

/// <summary>DTO for creating a new product — fully validated.</summary>
public class CreateProductDto
{
    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(200, MinimumLength = 2,
        ErrorMessage = "Name must be between 2 and 200 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, 999999.99,
        ErrorMessage = "Price must be between $0.01 and $999,999.99.")]
    public decimal Price { get; set; }

    [Range(0, 100000,
        ErrorMessage = "Stock quantity must be between 0 and 100,000.")]
    public int StockQuantity { get; set; }

    [Required(ErrorMessage = "CategoryId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a positive integer.")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "SupplierId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "SupplierId must be a positive integer.")]
    public int SupplierId { get; set; }
}

/// <summary>DTO for updating an existing product — fully validated.</summary>
public class UpdateProductDto
{
    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(200, MinimumLength = 2,
        ErrorMessage = "Name must be between 2 and 200 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, 999999.99,
        ErrorMessage = "Price must be between $0.01 and $999,999.99.")]
    public decimal Price { get; set; }

    [Range(0, 100000,
        ErrorMessage = "Stock quantity must be between 0 and 100,000.")]
    public int StockQuantity { get; set; }

    [Required(ErrorMessage = "CategoryId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a positive integer.")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "SupplierId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "SupplierId must be a positive integer.")]
    public int SupplierId { get; set; }
}

/// <summary>DTO for creating a supplier — demonstrates EmailAddress validation.</summary>
public class CreateSupplierDto
{
    [Required(ErrorMessage = "Supplier name is required.")]
    [StringLength(150, MinimumLength = 2,
        ErrorMessage = "Name must be between 2 and 150 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
    [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Please provide a valid phone number.")]
    [StringLength(20, ErrorMessage = "Phone cannot exceed 20 characters.")]
    public string? Phone { get; set; }
}

/// <summary>DTO for creating a category.</summary>
public class CreateCategoryDto
{
    [Required(ErrorMessage = "Category name is required.")]
    [StringLength(100, MinimumLength = 2,
        ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; }
}

/// <summary>Standard response DTO for products.</summary>
public class ProductResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
}
