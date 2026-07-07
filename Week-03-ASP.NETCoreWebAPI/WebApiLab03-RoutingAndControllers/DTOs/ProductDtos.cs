namespace WebApiLab03.DTOs;

// ============================================================
//  DATA TRANSFER OBJECTS (DTOs)
//  -----------------------------
//  DTOs are simple classes used to shape the data sent to/from
//  the API. They decouple the API contract from the database model.
//
//  Benefits:
//  - Hide internal entity structure from API consumers
//  - Include only the fields needed for each operation
//  - Prevent over-posting attacks (client sending extra fields)
// ============================================================

/// <summary>DTO for creating or updating a product.</summary>
public class ProductRequestDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int CategoryId { get; set; }
    public int SupplierId { get; set; }
}

/// <summary>DTO returned to the client — includes related names.</summary>
public class ProductResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
}
