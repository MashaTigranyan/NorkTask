using MariamApp.Data;
using MariamApp.Data.Entities;
using MariamApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MariamApp.Services;

public class SupplierService : ISupplierService
{
    private readonly DataContext _dbContext;

    public SupplierService(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Get all suppliers.
    /// </summary>
    public async Task<IEnumerable<Supplier>> GetSuppliersAsync()
    {
        return await _dbContext.Suppliers.ToListAsync();
    }

    /// <summary>
    /// Get a supplier by ID.
    /// </summary>
    public async Task<Supplier?> GetSupplierByIdAsync(int id)
    {
        return await _dbContext.Suppliers.FindAsync(id);
    }

    /// <summary>
    /// Add a new supplier.
    /// </summary>
    public async Task<Supplier> AddSupplierAsync(Supplier supplier)
    {
        _dbContext.Suppliers.Add(supplier);
        await _dbContext.SaveChangesAsync();
        return supplier;
    }

    /// <summary>
    /// Update an existing supplier.
    /// </summary>
    public async Task<Supplier?> UpdateSupplierAsync(int id, Supplier supplier)
    {
        var existingSupplier = await _dbContext.Suppliers.FindAsync(id);
        if (existingSupplier == null) return null;

        existingSupplier.Name = supplier.Name;
        existingSupplier.ContactInfo = supplier.ContactInfo;

        await _dbContext.SaveChangesAsync();
        return existingSupplier;
    }

    /// <summary>
    /// Delete a supplier.
    /// </summary>
    public async Task<bool> DeleteSupplierAsync(int id)
    {
        var supplier = await _dbContext.Suppliers.FindAsync(id);
        if (supplier == null) return false;

        _dbContext.Suppliers.Remove(supplier);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
