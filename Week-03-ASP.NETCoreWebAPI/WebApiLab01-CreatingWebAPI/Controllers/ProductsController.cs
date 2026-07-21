using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiLab01.Data;
using WebApiLab01.Models;





























namespace WebApiLab01.Controllers;




[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }






    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .ToListAsync();

        return Ok(products);
    }







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







    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {

        bool categoryExists = await _context.Categories.AnyAsync(c => c.Id == product.CategoryId);
        bool supplierExists = await _context.Suppliers.AnyAsync(s => s.Id == product.SupplierId);

        if (!categoryExists)
            return BadRequest(new { message = $"CategoryId={product.CategoryId} does not exist." });

        if (!supplierExists)
            return BadRequest(new { message = $"SupplierId={product.SupplierId} does not exist." });

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }
}
