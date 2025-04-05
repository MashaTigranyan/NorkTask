using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MariamApp.Data.Entities;

[Table("products")]
public class Product
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(200)]
    public string Name { get; set; }
    
    [Required]
    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    [Required]
    [ForeignKey("Supplier")]
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; }
    
    [Required]
    [Precision(18, 4)] 
    public decimal Price { get; set; }
    
    [Required]
    public int StockQuantity { get; set; }
    
}