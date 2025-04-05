using MariamApp.Data;
using MariamApp.Data.Entities;
using MariamApp.Helpers;
using MariamApp.Hub;
using MariamApp.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace MariamApp.Services;

public class TransactionService : ITransactionService
{
    private readonly DataContext _dbContext;
    private readonly SignalRService _signalRService;

    public TransactionService(DataContext dbContext, SignalRService signalRService)
    {
        _dbContext = dbContext;
        _signalRService = signalRService;
    }

    /// <summary>
    /// Record a purchase transaction (increase stock).
    /// </summary>
    public async Task<Transaction> RecordPurchaseAsync(int productId, int quantity)
    {
        var product = await _dbContext.Products.FindAsync(productId);
        if (product == null) throw new KeyNotFoundException("Product not found");
        
        var transaction = new Transaction
        {
            ProductId = productId,
            Quantity = quantity,
            Type = TransactionType.Purchase,
            TransactionDate = DateTime.UtcNow
        };
        
        product.StockQuantity += quantity;

        _dbContext.Transactions.Add(transaction);
        await _dbContext.SaveChangesAsync();
        
        SendStockUpdateNotification("purchase", quantity, product.Name, product.StockQuantity);

        return transaction;
    }

    /// <summary>
    /// Record a sale transaction (decrease stock).
    /// </summary>
    public async Task<Transaction> RecordSaleAsync(int productId, int quantity)
    {
        var product = await _dbContext.Products.FindAsync(productId);
        if (product == null) throw new KeyNotFoundException("Product not found");
        
        if (product.StockQuantity < quantity) throw new InvalidOperationException("Not enough stock for sale");
        
        var transaction = new Transaction
        {
            ProductId = productId,
            Quantity = quantity,
            Type = TransactionType.Sale,
            TransactionDate = DateTime.UtcNow
        };
        
        product.StockQuantity -= quantity;

        _dbContext.Transactions.Add(transaction);
        await _dbContext.SaveChangesAsync();

        SendStockUpdateNotification("sale", quantity, product.Name, product.StockQuantity);

        return transaction;
    }

    /// <summary>
    /// Get all transactions.
    /// </summary>
    public async Task<IEnumerable<Transaction>> GetTransactionsAsync()
    {
        return await _dbContext.Transactions
            .Include(t => t.Product)
            .ToListAsync();
    }

    /// <summary>
    /// Get transactions for a specific product.
    /// </summary>
    public async Task<IEnumerable<Transaction>> GetTransactionsByProductIdAsync(int productId)
    {
        return await _dbContext.Transactions
            .Where(t => t.ProductId == productId)
            .Include(t => t.Product)
            .ToListAsync();
    }

    private async Task SendStockUpdateNotification(
        string transactionType, 
        int transactionQuantity,
        string productName, 
        int stockQuantity)
    {
        await _signalRService.SendStockUpdateMessageToUser(transactionType, transactionQuantity, productName, stockQuantity);
    }
}
