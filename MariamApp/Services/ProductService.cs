using MariamApp.Data;
using MariamApp.Data.Entities;
using MariamApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MariamApp.Services;

public class ProductService : IProductService
{
    private readonly DataContext _dbContext;

    public ProductService(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Get all products.
    /// </summary>
    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .ToListAsync();
    }

    /// <summary>
    /// Get a product by ID.
    /// </summary>
    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Add a new product.
    /// </summary>
    public async Task<Product> AddProductAsync(Product product)
    {
        // Validate Category
        bool categoryExists = await _dbContext.Categories.AnyAsync(c => c.Id == product.CategoryId);
        if (!categoryExists)
            throw new KeyNotFoundException("CategoryId does not exist.");

        // Validate Supplier
        bool supplierExists = await _dbContext.Suppliers.AnyAsync(s => s.Id == product.SupplierId);
        if (!supplierExists)
            throw new KeyNotFoundException("SupplierId does not exist.");
        
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    /// <summary>
    /// Update an existing product.
    /// </summary>
    public async Task<Product?> UpdateProductAsync(int id, Product product)
    {
        var existingProduct = await _dbContext.Products.FindAsync(id);
        if (existingProduct == null) 
            throw new KeyNotFoundException("Product not found.");
        
        // Validate Category
        bool categoryExists = await _dbContext.Categories.AnyAsync(c => c.Id == product.CategoryId);
        if (!categoryExists)
            throw new KeyNotFoundException("CategoryId does not exist.");

        // Validate Supplier
        bool supplierExists = await _dbContext.Suppliers.AnyAsync(s => s.Id == product.SupplierId);
        if (!supplierExists)
            throw new KeyNotFoundException("SupplierId does not exist.");

        existingProduct.Name = product.Name;
        existingProduct.CategoryId = product.CategoryId;
        existingProduct.SupplierId = product.SupplierId;
        existingProduct.Price = product.Price;
        existingProduct.StockQuantity = product.StockQuantity;

        await _dbContext.SaveChangesAsync();
        return existingProduct;
    }

    /// <summary>
    /// Delete a product.
    /// </summary>
    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null) return false;

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Update stock for purchase or sale.
    /// </summary>
    public async Task<bool> UpdateStockAsync(int id, int quantityChange)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null) return false;

        product.StockQuantity += quantityChange;
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
