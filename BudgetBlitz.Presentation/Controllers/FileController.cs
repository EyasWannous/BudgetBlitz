﻿using BudgetBlitz.Application.Helper;
using BudgetBlitz.Application.IServices;
using BudgetBlitz.Domain.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using System.IO.Compression;
using Aspose.Cells;
using System.Text;
using System.Collections;

namespace BudgetBlitz.Presentation.Controllers;

[Authorize]
public class FileController(IUnitOfWork unitOfWork, IDataExportService dataExportService) : BaseContoller(unitOfWork)
{
    private readonly IDataExportService _dataExportService = dataExportService;

    [HttpGet("downloadexcel")]
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
