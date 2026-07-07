using Microsoft.AspNetCore.Mvc;
using WebApiLab04.DTOs;
using WebApiLab04.Interfaces;
using WebApiLab04.Models;

namespace WebApiLab04.Controllers;

// ============================================================
//  Lab 04 — Model Validation
//  ProductsController
// ============================================================
//
//  HOW VALIDATION WORKS WITH [ApiController]:
//  -------------------------------------------
//  1. Request arrives with JSON body
//  2. ASP.NET Core deserialises JSON → CreateProductDto
//  3. Data Annotations on the DTO are evaluated
//  4. If any annotation fails → ModelState.IsValid = false
//  5. [ApiController] automatically returns 400 Bad Request
//     with a ProblemDetails response listing all errors
//  6. Your action method is NOT called if validation fails
//
//  This means you do NOT need to write:
//      if (!ModelState.IsValid) return BadRequest(ModelState);
//  [ApiController] handles it automatically.

/// <summary>Products controller with full model validation.</summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>Retrieves all products.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products.Select(ToResponseDto));
    }

    /// <summary>Retrieves a product by ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest(new { message = "Id must be a positive integer." });

        var product = await _productService.GetProductByIdAsync(id);
        if (product is null)
            return NotFound(new { message = $"Product with Id={id} was not found." });

        return Ok(ToResponseDto(product));
    }

    /// <summary>
    /// Creates a new product.
    /// Validation is enforced by Data Annotations on CreateProductDto.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        // If we reach here, [ApiController] has already validated the DTO.
        // ModelState.IsValid is guaranteed to be true at this point.
        try
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
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Updates an existing product.
    /// Validation is enforced by Data Annotations on UpdateProductDto.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
    {
        try
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
            if (updated is null)
                return NotFound(new { message = $"Product with Id={id} was not found." });

            return Ok(ToResponseDto(updated));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Deletes a product by ID.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteProductAsync(id);
        if (!deleted)
            return NotFound(new { message = $"Product with Id={id} was not found." });

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
