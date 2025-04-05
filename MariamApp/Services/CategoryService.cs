using MariamApp.Data;
using MariamApp.Data.Entities;
using MariamApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MariamApp.Services;

public class CategoryService : ICategoryService
{
    private readonly DataContext _dbContext;

    public CategoryService(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Get all categories.
    /// </summary>
    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        return await _dbContext.Categories.ToListAsync();
    }

    /// <summary>
    /// Get a category by ID.
    /// </summary>
    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _dbContext.Categories.FindAsync(id);
    }

    /// <summary>
    /// Add a new category.
    /// </summary>
    public async Task<Category> AddCategoryAsync(Category category)
    {
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();
        return category;
    }

    /// <summary>
    /// Update an existing category.
    /// </summary>
    public async Task<Category?> UpdateCategoryAsync(int id, Category category)
    {
        var existingCategory = await _dbContext.Categories.FindAsync(id);
        if (existingCategory == null) return null;

        existingCategory.Name = category.Name;

        await _dbContext.SaveChangesAsync();
        return existingCategory;
    }

    /// <summary>
    /// Delete a category.
    /// </summary>
    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category == null) return false;

        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
