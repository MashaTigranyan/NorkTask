using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MariamApp.Data.Entities;

public enum TransactionType
{
    Purchase,
    Sale
}


[Table("transactions")]
public class Transaction
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public DateTime TransactionDate { get; set; }
    
    public int Quantity { get; set; }

    [Required]
    public TransactionType Type { get; set; } 
}