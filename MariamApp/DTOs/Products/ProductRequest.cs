using System.ComponentModel.DataAnnotations;

namespace MariamApp.DTOs.Products;

public class ProductRequest
{
    public string Name { get; set; }
    
    [Required(ErrorMessage = "CategoryId is required")]
    public int CategoryId { get; set; }
    
    [Required(ErrorMessage = "SupplierId is required")]
    public int SupplierId { get; set; }
    
    [Required(ErrorMessage = "Price must be greater than zero")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "Quantity must be greater than zero")]
    public int StockQuantity { get; set; }
}