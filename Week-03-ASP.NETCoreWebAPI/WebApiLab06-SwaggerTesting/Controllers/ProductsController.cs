using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApiLab06.DTOs;
using WebApiLab06.Interfaces;
using WebApiLab06.Models;

namespace WebApiLab06.Controllers;






































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


    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products.Select(ToResponseDto));
    }



    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return Ok(ToResponseDto(product));
    }



    [HttpGet("category/{categoryId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ProductResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        var products = await _productService.GetProductsByCategoryAsync(categoryId);
        return Ok(products.Select(ToResponseDto));
    }








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
