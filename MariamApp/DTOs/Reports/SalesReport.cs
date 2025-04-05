namespace MariamApp.DTOs.Reports;

public class SalesReport
{
    public DateTime Date { get; set; }
    public string ProductName { get; set; }
    public string Category { get; set; }
    public int QuantitySold { get; set; }
    public string ProductPrice { get; set; }
    public string TotalPrice { get; set; }
 
}