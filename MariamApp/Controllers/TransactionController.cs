using MariamApp.Interfaces;
using MariamApp.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MariamApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    /// <summary>
    /// Record a purchase transaction (increase stock).
    /// </summary>
    [HttpPost("purchase")]
    public async Task<IActionResult> RecordPurchase([FromBody] TransactionRequest request)
    {
        if (request.Quantity <= 0)
            return BadRequest("Quantity must be greater than zero");

        try
        {
            var transaction = await _transactionService.RecordPurchaseAsync(request.ProductId, request.Quantity);
            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Product not found" });
        }
    }

    /// <summary>
    /// Record a sale transaction (decrease stock).
    /// </summary>
    [HttpPost("sale")]
    public async Task<IActionResult> RecordSale([FromBody] TransactionRequest request)
    {
        if (request.Quantity <= 0)
            return BadRequest("Quantity must be greater than zero");

        try
        {
            var transaction = await _transactionService.RecordSaleAsync(request.ProductId, request.Quantity);
            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Product not found" });
        }
        catch (InvalidOperationException)
        {
            return BadRequest(new { message = "Not enough stock for sale" });
        }
    }

    /// <summary>
    /// Get all transactions.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetTransactions()
    {
        return Ok(await _transactionService.GetTransactionsAsync());
    }

    /// <summary>
    /// Get transactions for a specific product.
    /// </summary>
    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetTransactionsByProductId(int productId)
    {
        var transactions = await _transactionService.GetTransactionsByProductIdAsync(productId);
        if (!transactions.Any())
            return NotFound(new { message = "No transactions found for this product" });

        return Ok(transactions);
    }

    /// <summary>
    /// Get transaction by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransactionById(int id)
    {
        var transaction = await _transactionService.GetTransactionsAsync();
        return transaction == null ? NotFound() : Ok(transaction);
    }
}
