using MariamApp.Data;
using MariamApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using MariamApp.Data.Entities;
using MariamApp.DTOs.Reports;
using MariamApp.Helpers;
using MariamApp.Hub;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace MariamApp.Services;

public class ReportService : IReportService
{
    private readonly DataContext _dbContext;
    private readonly IHttpContextAccessor _httpContext;
    private readonly SignalRService _signalRService;
    
    public ReportService(
        DataContext dbContext, 
        IHttpContextAccessor httpContext,
        SignalRService signalRService)
    {
        _dbContext = dbContext;
        _httpContext = httpContext;
        _signalRService = signalRService;
    }

    public async Task<BaseReport> GenerateSalesReportCsvAsync()
    {
        var salesData = await GetSalesDataAsync();
        var fileName = "SalesReport.csv";
        
        var result = new BaseReport
        {
            Bytes = CsvGenerator.GenerateCsv(salesData),
            FileName = fileName
                
        };

        SendFileReadyNotification(fileName);
        return result;
    }

    public async Task<BaseReport> GeneratePurchaseReportCsvAsync()
    {
        var purchaseData = await GetPurchaseDataAsync();
        var fileName = "PurchaseReport.csv";
        
        var result = new BaseReport
        {
            Bytes = CsvGenerator.GenerateCsv(purchaseData),
            FileName = fileName
                
        };

        SendFileReadyNotification(fileName);
        
        return result;
    }

    public async Task<BaseReport> GenerateStockReportCsvAsync()
    {
        var stockData = await GetStockDataAsync();
        var fileName = "StockReport.csv";
        
        var result = new BaseReport
        {
            Bytes = CsvGenerator.GenerateCsv(stockData),
            FileName = fileName
                
        };

        SendFileReadyNotification(fileName);
        
        return result;
    }
    
    public async Task<BaseReport> GenerateSalesReportPdfAsync()
    {
        var salesData = await GetSalesDataAsync();
        var fileName = "SalesReport.pdf";
        
        var result = new BaseReport
        {
            Bytes = PdfGenerator.GeneratePdf("Sales Report", salesData),
            FileName = fileName
                
        };

        SendFileReadyNotification(fileName);
        
        return result;
    }

    public async Task<BaseReport> GeneratePurchaseReportPdfAsync()
    {
        var purchaseData = await GetPurchaseDataAsync();
        var fileName = "PurchaseReport.pdf";
        
        var result = new BaseReport
        {
            Bytes = PdfGenerator.GeneratePdf("Purchase Report", purchaseData),
            FileName = fileName
                
        };

        SendFileReadyNotification(fileName);
        
        return result; ;
    }

    public async Task<BaseReport> GenerateStockReportPdfAsync()
    {
        var stockData = await GetStockDataAsync();
        var fileName = "StockReport.pdf";
        
        var result = new BaseReport
        {
            Bytes = PdfGenerator.GeneratePdf("Stock Report", stockData),
            FileName = fileName
                
        };

        SendFileReadyNotification(fileName);
        
        return result; ;
    }
    
    private async Task<List<SalesReport>> GetSalesDataAsync()
    {
        var res = await _dbContext.Transactions
            .Where(t => t.Type == TransactionType.Sale)
            .Select(t => new SalesReport
            {
                Date = t.TransactionDate,
                ProductName = t.Product.Name,
                Category = t.Product.Category.Name,
                QuantitySold = t.Quantity,
                ProductPrice = t.Product.Price.ToString("F2"),
                TotalPrice = (t.Product.Price * t.Quantity).ToString("F2")
            })
            .ToListAsync();
        return  res;
    }
    
    private async Task<List<PurchaseReport>> GetPurchaseDataAsync()
    {
        return await _dbContext.Transactions
            .Where(t => t.Type == TransactionType.Purchase)
            .Select(t => new PurchaseReport
            {
                Date = t.TransactionDate,
                ProductName = t.Product.Name,
                Supplier = t.Product.Supplier.Name,
                QuantityPurchased = t.Quantity,
                ProductPrice = t.Product.Price.ToString("F2"),
                TotalCost = (t.Product.Price * t.Quantity).ToString("F2")
            })
            .ToListAsync();
    }
    
    private async Task<List<StockReport>> GetStockDataAsync()
    {
        return await _dbContext.Products
            .Select(p => new StockReport
            {
                ProductName = p.Name,
                Category = p.Category.Name,
                CurrentStock = p.StockQuantity
            })
            .ToListAsync();
    }
    
        
    private async Task SendFileReadyNotification(string fileName)
    {
        var id = _httpContext.HttpContext.User.Identity.Name;

        await _signalRService.SendReaportReadyMessageToUser(id,$"{fileName} file ready for download");
    }
}
