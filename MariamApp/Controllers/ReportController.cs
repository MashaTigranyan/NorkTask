using MariamApp.Helpers;
using MariamApp.Hub;
using MariamApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MariamApp.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// Generate Sales report as csv.
    /// </summary>
    [HttpGet("sales/csv")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> ExportSalesReport()
    {
        var fileDetails = await _reportService.GenerateSalesReportCsvAsync(); ;
        return File(fileDetails.Bytes, "text/csv", fileDetails.FileName);
    }

    /// <summary>
    /// Generate Purchases report as csv.
    /// </summary>
    [HttpGet("purchases/csv")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> ExportPurchaseReport()
    {
        var fileDetails = await _reportService.GeneratePurchaseReportCsvAsync();
        return File(fileDetails.Bytes, "text/csv", fileDetails.FileName);
    }

    /// <summary>
    /// Generate Stock status report as csv.
    /// </summary>
    [HttpGet("stock/csv")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> ExportStockReport()
    {
        var fileDetails = await _reportService.GenerateStockReportCsvAsync();
        return File(fileDetails.Bytes, "text/csv", fileDetails.FileName);
    }
    
    /// <summary>
    /// Generate Sales report as pdf.
    /// </summary>
    [HttpGet("sales/pdf")]
    public async Task<IActionResult> ExportSalesReportPdf()
    {
        var fileDetails = await _reportService.GenerateSalesReportPdfAsync();
        return File(fileDetails.Bytes, "application/pdf", fileDetails.FileName);
    }
    
    /// <summary>
    /// Generate Purchases report as pdf.
    /// </summary>
    [HttpGet("purchases/pdf")]
    public async Task<IActionResult> ExportPurchaseReportPdf()
    {
        var fileDetails = await _reportService.GeneratePurchaseReportPdfAsync();
        return File(fileDetails.Bytes, "application/pdf", fileDetails.FileName);
    }

    /// <summary>
    /// Generate Stock status report as pdf.
    /// </summary>
    [HttpGet("stock/pdf")]
    public async Task<IActionResult> ExportStockReportPdf()
    {
        var fileDetails = await _reportService.GenerateStockReportPdfAsync();
        return File(fileDetails.Bytes, "application/pdf", fileDetails.FileName);
    }
}

