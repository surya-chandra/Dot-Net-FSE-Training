using Microsoft.AspNetCore.Mvc;
using WebApiLab03.DTOs;
using WebApiLab03.Interfaces;
using WebApiLab03.Models;

namespace WebApiLab03.Controllers;

// ============================================================
//  Lab 03 — Routing & Controllers
//  ProductsController
// ============================================================
//
//  ATTRIBUTE ROUTING:
//  ------------------
//  [Route("api/[controller]")]  — [controller] is replaced with
//  the class name minus "Controller" → "Products"
//  Final base URL: /api/products
//
//  ROUTE PARAMETERS:
//  -----------------
//  {id:int}  — captures an integer segment from the URL
//  GET /api/products/5  → id = 5
//
//  QUERY PARAMETERS:
//  -----------------
//  Bound from the URL query string automatically:
//  GET /api/products/search?name=laptop&minPrice=100
//  → name = "laptop", minPrice = 100
//
//  ROUTE CONSTRAINTS:
//  ------------------
//  :int    — only matches integers
//  :guid   — only matches GUIDs
//  :alpha  — only matches alphabetic strings
//  :minlength(3) — minimum string length

/// <summary>
/// Full CRUD controller for Products with advanced routing examples.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // -------------------------------------------------------
    // GET /api/products
    // Returns all products as response DTOs
    // -------------------------------------------------------
    /// <summary>Retrieves all products.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        var response = products.Select(MapToResponseDto);
        return Ok(response);
    }

    // -------------------------------------------------------
    // GET /api/products/{id}
    // Route parameter: {id:int} — only matches integers
    // -------------------------------------------------------
    /// <summary>Retrieves a product by its ID.</summary>
    /// <param name="id">Product ID (integer route parameter).</param>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product is null)
            return NotFound(new { message = $"Product with Id={id} was not found." });

        return Ok(MapToResponseDto(product));
    }

    // -------------------------------------------------------
    // GET /api/products/category/{categoryId}
    // Nested route — products filtered by category
    // -------------------------------------------------------
    /// <summary>Retrieves all products in a specific category.</summary>
    /// <param name="categoryId">The category ID.</param>
    [HttpGet("category/{categoryId:int}")]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        var products = await _productService.GetProductsByCategoryAsync(categoryId);
        var response = products.Select(MapToResponseDto);
        return Ok(response);
    }

    // -------------------------------------------------------
    // GET /api/products/search?name=laptop&minPrice=50&maxPrice=500
    // Query parameters — all optional, bound from query string
    // -------------------------------------------------------
    /// <summary>Searches products by name and/or price range.</summary>
    /// <param name="name">Optional name filter (partial match).</param>
    /// <param name="minPrice">Optional minimum price.</param>
    /// <param name="maxPrice">Optional maximum price.</param>
    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? name,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice)
    {
        if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
            return BadRequest(new { message = "minPrice cannot be greater than maxPrice." });

        var products = await _productService.SearchProductsAsync(name, minPrice, maxPrice);
        var response = products.Select(MapToResponseDto);
        return Ok(response);
    }

    // -------------------------------------------------------
    // POST /api/products
    // [FromBody] — binds the JSON request body to the DTO
    // -------------------------------------------------------
    /// <summary>Creates a new product.</summary>
    /// <param name="dto">Product data from the request body.</param>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductRequestDto dto)
    {
        try
        {
            var product = MapFromRequestDto(dto);
            var created = await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToResponseDto(created));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // -------------------------------------------------------
    // PUT /api/products/{id}
    // Full update — replaces all fields
    // -------------------------------------------------------
    /// <summary>Updates an existing product.</summary>
    /// <param name="id">Product ID from the route.</param>
    /// <param name="dto">Updated product data from the request body.</param>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductRequestDto dto)
    {
        try
        {
            var product = MapFromRequestDto(dto);
            var updated = await _productService.UpdateProductAsync(id, product);
            if (updated is null)
                return NotFound(new { message = $"Product with Id={id} was not found." });

            return Ok(MapToResponseDto(updated));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // -------------------------------------------------------
    // DELETE /api/products/{id}
    // Returns 204 No Content on success
    // -------------------------------------------------------
    /// <summary>Deletes a product by ID.</summary>
    /// <param name="id">Product ID to delete.</param>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteProductAsync(id);
        if (!deleted)
            return NotFound(new { message = $"Product with Id={id} was not found." });

        return NoContent();
    }

    // -------------------------------------------------------
    // Private helpers — manual mapping (no AutoMapper needed)
    // -------------------------------------------------------
    private static ProductResponseDto MapToResponseDto(Product p) => new()
    {
        Id           = p.Id,
        Name         = p.Name,
        Price        = p.Price,
        StockQuantity = p.StockQuantity,
        CategoryName = p.Category?.Name ?? string.Empty,
        SupplierName = p.Supplier?.Name ?? string.Empty
    };

    private static Product MapFromRequestDto(ProductRequestDto dto) => new()
    {
        Name          = dto.Name,
        Price         = dto.Price,
        StockQuantity = dto.StockQuantity,
        CategoryId    = dto.CategoryId,
        SupplierId    = dto.SupplierId
    };
}
