using MariamApp.DTOs.Reports;

namespace MariamApp.Interfaces;

public interface IReportService
{
    Task<BaseReport> GenerateSalesReportCsvAsync();
    Task<BaseReport> GeneratePurchaseReportCsvAsync();
    Task<BaseReport> GenerateStockReportCsvAsync();
    
    
    Task<BaseReport> GenerateSalesReportPdfAsync();
    Task<BaseReport> GeneratePurchaseReportPdfAsync();
    Task<BaseReport> GenerateStockReportPdfAsync();
}