using MariamApp.Data.Entities;

namespace MariamApp.Interfaces;

public interface ISupplierService
{
    Task<IEnumerable<Supplier>> GetSuppliersAsync();
    Task<Supplier?> GetSupplierByIdAsync(int id);
    Task<Supplier> AddSupplierAsync(Supplier supplier);
    Task<Supplier?> UpdateSupplierAsync(int id, Supplier supplier);
    Task<bool> DeleteSupplierAsync(int id);
}