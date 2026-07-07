using Microsoft.AspNetCore.Mvc;
using WebApiLab05.DTOs;
using WebApiLab05.Interfaces;
using WebApiLab05.Models;

namespace WebApiLab05.Controllers;

// ============================================================
//  Lab 05 — Exception Handling
//  ProductsController
// ============================================================
//
//  Notice how CLEAN this controller is — no try-catch blocks.
//  All exceptions are thrown by the Service layer and caught
//  by GlobalExceptionMiddleware, which returns the correct
//  HTTP status code and JSON error response automatically.
//
//  HTTP STATUS CODES RETURNED:
//  ---------------------------
//  200 OK           — successful GET
//  201 Created      — successful POST
//  204 No Content   — successful DELETE
//  400 Bad Request  — ValidationException from service
//  404 Not Found    — NotFoundException from service
//  500 Server Error — any other unhandled exception

/// <summary>Products controller — clean, exception-free actions.</summary>
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

    /// <summary>Retrieves a product by ID. Returns 404 if not found.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        // NotFoundException thrown by service → middleware returns 404
        var product = await _productService.GetProductByIdAsync(id);
        return Ok(ToResponseDto(product));
    }

    /// <summary>Creates a new product. Returns 400 if validation fails.</summary>
    [HttpPost]
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

        // ValidationException thrown by service → middleware returns 400
        var created = await _productService.CreateProductAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToResponseDto(created));
    }

    /// <summary>Updates an existing product. Returns 404 if not found.</summary>
    [HttpPut("{id:int}")]
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

    /// <summary>Deletes a product. Returns 404 if not found.</summary>
    [HttpDelete("{id:int}")]
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
