using Microsoft.AspNetCore.Mvc;
using WebApiLab04.DTOs;
using WebApiLab04.Interfaces;
using WebApiLab04.Models;

namespace WebApiLab04.Controllers;

/// <summary>
/// Suppliers controller — demonstrates [EmailAddress] and [Phone] validation.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierRepository _supplierRepository;

    public SuppliersController(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    /// <summary>Retrieves all suppliers.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var suppliers = await _supplierRepository.GetAllAsync();
        return Ok(suppliers);
    }

    /// <summary>Retrieves a supplier by ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);
        if (supplier is null)
            return NotFound(new { message = $"Supplier with Id={id} was not found." });

        return Ok(supplier);
    }

    /// <summary>
    /// Creates a new supplier.
    /// Demonstrates [EmailAddress] and [Phone] validation on CreateSupplierDto.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSupplierDto dto)
    {
        // [ApiController] has already validated the DTO here.
        // Invalid email → 400 Bad Request with error details automatically.
        var supplier = new Supplier
        {
            Name  = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone
        };

        var created = await _supplierRepository.CreateAsync(supplier);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
