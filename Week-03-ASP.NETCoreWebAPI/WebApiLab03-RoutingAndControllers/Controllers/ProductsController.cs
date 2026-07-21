using Microsoft.AspNetCore.Mvc;
using WebApiLab03.DTOs;
using WebApiLab03.Interfaces;
using WebApiLab03.Models;

namespace WebApiLab03.Controllers;































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
        var response = products.Select(MapToResponseDto);
        return Ok(response);
    }






    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product is null)
            return NotFound(new { message = $"Product with Id={id} was not found." });

        return Ok(MapToResponseDto(product));
    }






    [HttpGet("category/{categoryId:int}")]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        var products = await _productService.GetProductsByCategoryAsync(categoryId);
        var response = products.Select(MapToResponseDto);
        return Ok(response);
    }








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






    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteProductAsync(id);
        if (!deleted)
            return NotFound(new { message = $"Product with Id={id} was not found." });

        return NoContent();
    }



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
