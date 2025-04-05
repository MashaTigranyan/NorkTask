namespace MariamApp.DTOs.Reports;

public class PurchaseReport
{
    public DateTime Date { get; set; }
    public string ProductName { get; set; }
    public string Supplier { get; set; }
    public int QuantityPurchased { get; set; }
    public string ProductPrice { get; set; }
    public string TotalCost { get; set; }
}