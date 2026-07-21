using Microsoft.AspNetCore.Mvc;
using WebApiLab05.DTOs;
using WebApiLab05.Interfaces;
using WebApiLab05.Models;

namespace WebApiLab05.Controllers;



















[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products.Select(ToResponseDto));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {

        var product = await _productService.GetProductByIdAsync(id);
        return Ok(ToResponseDto(product));
    }

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

        var created = await _productService.CreateProductAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToResponseDto(created));
    }

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
