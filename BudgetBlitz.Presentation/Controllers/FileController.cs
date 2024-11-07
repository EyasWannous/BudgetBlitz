using BudgetBlitz.Application.Helper;
using BudgetBlitz.Application.IServices;
using BudgetBlitz.Domain.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBlitz.Presentation.Controllers;

[Authorize]
public class FileController(IUnitOfWork unitOfWork, IDataExportService dataExportService) : BaseController(unitOfWork)
{
    private readonly IDataExportService _dataExportService = dataExportService;

    [HttpGet("downloadُExcel")]
    public async Task<IActionResult> DownloadExcel(CancellationToken cancellationToken = default)
    {
        var files = await _dataExportService.ExportExcelAsync(HttpContext.User, cancellationToken);

        var result = await ZipCompresser.CompressFiles(files, "xlsx", cancellationToken);

        return File(result, "application/zip", "reports.zip");
    }


    [HttpGet("downloadCSV")]
    public async Task<IActionResult> DownloadCSV(CancellationToken cancellationToken = default)
    {
        var files = await _dataExportService.ExportExcelAsync(HttpContext.User, cancellationToken);

        var workBooks = await _dataExportService.ExportCSVAsync(files);

        var result = await ZipCompresser.CompressFiles(workBooks, "csv", cancellationToken);

        return File(result, "application/zip", "reports.zip");
    }
}
