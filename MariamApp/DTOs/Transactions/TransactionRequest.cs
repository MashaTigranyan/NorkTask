using System.ComponentModel.DataAnnotations;

namespace MariamApp.RequestModels;

public class TransactionRequest
{
    [Required]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Quantity must be greater than zero")]
    public int Quantity { get; set; }
}