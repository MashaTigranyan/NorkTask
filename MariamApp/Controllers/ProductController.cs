using AutoMapper;
using MariamApp.Data.Entities;
using MariamApp.DTOs.Products;
using MariamApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MariamApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Get all products.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        return Ok(await _productService.GetProductsAsync());
    }
    
    /// <summary>
    /// Get product by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound(new { message = "Product not found" });

        return Ok(product);
    }
    
    /// <summary>
    /// Create a new product.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] ProductRequest productDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var product = _mapper.Map<Product>(productDTO);
        var createdProduct = await _productService.AddProductAsync(product);
        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }

    /// <summary>
    /// Update an existing product.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductRequest productDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var product = _mapper.Map<Product>(productDTO);
        var updatedProduct = await _productService.UpdateProductAsync(id, product);
        if (updatedProduct == null)
            return NotFound(new { message = "Product not found" });

        return Ok(updatedProduct);
    }
    
    /// <summary>
    /// Delete a product.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await _productService.DeleteProductAsync(id);
        if (!deleted)
            return NotFound(new { message = "Product not found" });

        return NoContent();
    }
    
    /// <summary>
    /// Update stock for purchase or sale.
    /// </summary>
    [HttpPatch("{id}/stock")]
    public async Task<IActionResult> UpdateStock(int id, [FromBody] int quantityChange)
    {
        var updated = await _productService.UpdateStockAsync(id, quantityChange);
        if (!updated)
            return NotFound(new { message = "Product not found" });

        return Ok(new { message = "Stock updated successfully" });
    }
}