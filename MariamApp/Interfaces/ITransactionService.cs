using MariamApp.Data.Entities;

namespace MariamApp.Interfaces;

public interface ITransactionService
{
    Task<Transaction> RecordPurchaseAsync(int productId, int quantity);
    Task<Transaction> RecordSaleAsync(int productId, int quantity);
    Task<IEnumerable<Transaction>> GetTransactionsAsync();
    Task<IEnumerable<Transaction>> GetTransactionsByProductIdAsync(int productId);
}
