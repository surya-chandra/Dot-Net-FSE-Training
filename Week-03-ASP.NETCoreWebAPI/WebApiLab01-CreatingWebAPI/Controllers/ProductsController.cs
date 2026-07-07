using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiLab01.Data;
using WebApiLab01.Models;

// ============================================================
//  Lab 01 — Creating a Web API
//  ProductsController
// ============================================================
//
//  WEB API BASICS:
//  ---------------
//  A Web API exposes HTTP endpoints that clients (browsers,
//  mobile apps, other services) can call over the network.
//
//  HTTP METHODS:
//  -------------
//  GET    — retrieve data        (safe, idempotent)
//  POST   — create new resource  (not idempotent)
//  PUT    — update resource      (idempotent)
//  DELETE — remove resource      (idempotent)
//
//  HTTP STATUS CODES:
//  ------------------
//  200 OK           — success
//  201 Created      — resource created
//  400 Bad Request  — invalid input
//  404 Not Found    — resource not found
//  500 Server Error — unexpected server error
//
//  [ApiController]  — enables automatic model validation,
//                     binding source inference, and problem details
//  [Route]          — defines the URL pattern for this controller

namespace WebApiLab01.Controllers;

/// <summary>
/// Manages product resources in the Retail Inventory System.
/// Base route: /api/products
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    // Constructor injection — ASP.NET Core DI provides the DbContext
    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // -------------------------------------------------------
    // GET /api/products
    // Returns all products with their Category and Supplier
    // -------------------------------------------------------

    /// <summary>Retrieves all products.</summary>
    /// <returns>List of all products.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .ToListAsync();

        // 200 OK with the product list as JSON in the response body
        return Ok(products);
    }

    // -------------------------------------------------------
    // GET /api/products/{id}
    // Returns a single product by its primary key
    // -------------------------------------------------------

    /// <summary>Retrieves a product by ID.</summary>
    /// <param name="id">The product ID.</param>
    /// <returns>The matching product, or 404 if not found.</returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            return NotFound(new { message = $"Product with Id={id} was not found." });

        return Ok(product);
    }

    // -------------------------------------------------------
    // POST /api/products
    // Creates a new product from the JSON request body
    // -------------------------------------------------------

    /// <summary>Creates a new product.</summary>
    /// <param name="product">Product data from the request body.</param>
    /// <returns>201 Created with the new product.</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        // Validate that the referenced Category and Supplier exist
        bool categoryExists = await _context.Categories.AnyAsync(c => c.Id == product.CategoryId);
        bool supplierExists = await _context.Suppliers.AnyAsync(s => s.Id == product.SupplierId);

        if (!categoryExists)
            return BadRequest(new { message = $"CategoryId={product.CategoryId} does not exist." });

        if (!supplierExists)
            return BadRequest(new { message = $"SupplierId={product.SupplierId} does not exist." });

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // 201 Created — includes a Location header pointing to the new resource
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }
}
