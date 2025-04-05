using AutoMapper;
using MariamApp.Data.Entities;
using MariamApp.DTOs.Categories;
using MariamApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MariamApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Get all categories.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        return Ok(await _categoryService.GetCategoriesAsync());
    }
    
    /// <summary>
    /// Get category by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound(new { message = "Category not found" });

        return Ok(category);
    }

    /// <summary>
    /// Create a new category.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddCategory(CategoryRequest categoryDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var category = _mapper.Map<Category>(categoryDTO);
        var createdCategory = await _categoryService.AddCategoryAsync(category);
        return CreatedAtAction(nameof(GetCategories), new { id = createdCategory.Id }, createdCategory);
    }
    
    /// <summary>
    /// Update an existing category.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequest categoryDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var category = _mapper.Map<Category>(categoryDTO);
        var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
        if (existingCategory == null)
            return NotFound(new { message = "Category not found" });

        category.Id = id;
        
        return Ok(await _categoryService.UpdateCategoryAsync(id, category));
    }

    /// <summary>
    /// Delete a category.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var deleted = await _categoryService.DeleteCategoryAsync(id);
        if (!deleted)
            return NotFound(new { message = "Category not found" });

        return NoContent();
    }
}
