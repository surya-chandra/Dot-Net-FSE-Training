using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApiLab06.DTOs;
using WebApiLab06.Interfaces;
using WebApiLab06.Models;

namespace WebApiLab06.Controllers;

// ============================================================
//  Lab 06 — Swagger & API Testing
//  ProductsController
// ============================================================
//
//  SWAGGER / OPENAPI DOCUMENTATION:
//  ----------------------------------
//  Swagger reads XML comments (/// <summary>) and attributes
//  like [ProducesResponseType] to generate interactive API docs.
//
//  KEY SWAGGER ATTRIBUTES:
//  -----------------------
//  [ProducesResponseType(StatusCodes.Status200OK)]
//      → Documents that this endpoint can return 200
//  [Produces("application/json")]
//      → Documents the response content type
//  [Consumes("application/json")]
//      → Documents the request content type
//  /// <summary> XML comments
//      → Appear as descriptions in Swagger UI
//  /// <param name="id"> XML comments
//      → Appear as parameter descriptions
//  /// <returns> XML comments
//      → Appear as response descriptions
//
//  HOW TO TEST WITH SWAGGER UI:
//  ----------------------------
//  1. Run the project (dotnet run)
//  2. Navigate to https://localhost:{port}/swagger
//  3. Click any endpoint to expand it
//  4. Click "Try it out"
//  5. Fill in parameters / request body
//  6. Click "Execute"
//  7. View the response code and body

/// <summary>
/// Manages product resources in the Retail Inventory System.
/// Provides full CRUD operations plus search and category filtering.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>Retrieves all products in the inventory.</summary>
    /// <returns>A list of all products with category and supplier names.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products.Select(ToResponseDto));
    }

    /// <summary>Retrieves a single product by its unique ID.</summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>The product matching the given ID.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return Ok(ToResponseDto(product));
    }

    /// <summary>Retrieves all products belonging to a specific category.</summary>
    /// <param name="categoryId">The category ID to filter by.</param>
    /// <returns>Products in the specified category.</returns>
    [HttpGet("category/{categoryId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ProductResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        var products = await _productService.GetProductsByCategoryAsync(categoryId);
        return Ok(products.Select(ToResponseDto));
    }

    /// <summary>
    /// Searches products by name and/or price range.
    /// All parameters are optional — omit any to skip that filter.
    /// </summary>
    /// <param name="name">Partial product name to search for.</param>
    /// <param name="minPrice">Minimum price (inclusive).</param>
    /// <param name="maxPrice">Maximum price (inclusive).</param>
    /// <returns>Products matching the search criteria.</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<ProductResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search(
        [FromQuery] string? name,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice)
    {
        if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
            return BadRequest(new { message = "minPrice cannot be greater than maxPrice." });

        var products = await _productService.SearchProductsAsync(name, minPrice, maxPrice);
        return Ok(products.Select(ToResponseDto));
    }

    /// <summary>
    /// Creates a new product in the inventory.
    /// CategoryId and SupplierId must reference existing records.
    /// </summary>
    /// <param name="dto">The product data to create.</param>
    /// <returns>The newly created product with its assigned ID.</returns>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        var product = new Product
        {
            Name          = dto.Name,
            Price         = dto.Price,
            StockQuantity = dto.StockQuantity,
            CategoryId    = dto.CategoryId,
            SupplierId    = dto.SupplierId
        };

        var created = await _productService.CreateProductAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToResponseDto(created));
    }

    /// <summary>
    /// Updates all fields of an existing product.
    /// The product must exist; otherwise 404 is returned.
    /// </summary>
    /// <param name="id">The ID of the product to update.</param>
    /// <param name="dto">The updated product data.</param>
    /// <returns>The updated product.</returns>
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
    {
        var product = new Product
        {
            Name          = dto.Name,
            Price         = dto.Price,
            StockQuantity = dto.StockQuantity,
            CategoryId    = dto.CategoryId,
            SupplierId    = dto.SupplierId
        };

        var updated = await _productService.UpdateProductAsync(id, product);
        return Ok(ToResponseDto(updated));
    }

    /// <summary>
    /// Permanently deletes a product from the inventory.
    /// Returns 204 No Content on success.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }

    private static ProductResponseDto ToResponseDto(Product p) => new()
    {
        Id            = p.Id,
        Name          = p.Name,
        Price         = p.Price,
        StockQuantity = p.StockQuantity,
        CategoryName  = p.Category?.Name ?? string.Empty,
        SupplierName  = p.Supplier?.Name ?? string.Empty
    };
}
