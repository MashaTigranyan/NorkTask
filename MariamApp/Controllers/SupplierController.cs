using AutoMapper;
using MariamApp.Data.Entities;
using MariamApp.DTOs.Suppliers;
using MariamApp.Interfaces;
using MariamApp.Mappings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MariamApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _supplierService;
    private readonly IMapper _mapper;

    public SupplierController(ISupplierService supplierService, IMapper mapper)
    {
        _supplierService = supplierService;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Get all suppliers.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetSuppliers()
    {
        return Ok(await _supplierService.GetSuppliersAsync());
    }

    /// <summary>
    /// Get supplier by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSupplierById(int id)
    {
        var supplier = await _supplierService.GetSupplierByIdAsync(id);
        if (supplier == null)
            return NotFound(new { message = "Supplier not found" });

        return Ok(supplier);
    }

    /// <summary>
    /// Create a new supplier.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddSupplier([FromBody] SupplierRequest supplierDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var supplier = _mapper.Map<Supplier>(supplierDTO);
        var createdSupplier = await _supplierService.AddSupplierAsync(supplier);
        return CreatedAtAction(nameof(GetSupplierById), new { id = createdSupplier.Id }, createdSupplier);
    }

    /// <summary>
    /// Update an existing supplier.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSupplier(int id, [FromBody] SupplierRequest supplierDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var supplier = _mapper.Map<Supplier>(supplierDTO);
        var updatedSupplier = await _supplierService.UpdateSupplierAsync(id, supplier);
        if (updatedSupplier == null)
            return NotFound(new { message = "Supplier not found" });

        return Ok(updatedSupplier);
    }

    /// <summary>
    /// Delete a supplier.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        var deleted = await _supplierService.DeleteSupplierAsync(id);
        if (!deleted)
            return NotFound(new { message = "Supplier not found" });

        return NoContent();
    }
}