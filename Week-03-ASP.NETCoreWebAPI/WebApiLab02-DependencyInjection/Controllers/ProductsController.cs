using Microsoft.AspNetCore.Mvc;
using WebApiLab02.Interfaces;
using WebApiLab02.Models;

namespace WebApiLab02.Controllers;

// ============================================================
//  Lab 02 — Dependency Injection
//  ProductsController
// ============================================================
//
//  The Controller now depends on IProductService (interface),
//  NOT on ProductService (concrete class).
//
//  DEPENDENCY INJECTION FLOW:
//  --------------------------
//  Request arrives
//      → ASP.NET Core creates ProductsController
//      → Injects IProductService  (resolves to ProductService)
//      → ProductService needs IProductRepository
//      → Injects IProductRepository  (resolves to ProductRepository)
//      → ProductRepository needs ApplicationDbContext
//      → Injects ApplicationDbContext  (from AddDbContext)
//
//  All of this is wired up in Program.cs with:
//      builder.Services.AddScoped<IProductRepository, ProductRepository>();
//      builder.Services.AddScoped<IProductService, ProductService>();

/// <summary>
/// Manages product resources. Depends on IProductService via DI.
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

    /// <summary>Retrieves all products.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    /// <summary>Retrieves a product by ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product is null)
            return NotFound(new { message = $"Product with Id={id} was not found." });

        return Ok(product);
    }

    /// <summary>Creates a new product.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        try
        {
            var created = await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Updates an existing product.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product product)
    {
        try
        {
            var updated = await _productService.UpdateProductAsync(id, product);
            if (updated is null)
                return NotFound(new { message = $"Product with Id={id} was not found." });

            return Ok(updated);
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

        return NoContent();  // 204 No Content — success with no response body
    }
}
