using Aspose.Cells;
using BudgetBlitz.Application.Helper;
using BudgetBlitz.Application.IServices;
using BudgetBlitz.Domain.Abstractions;
using BudgetBlitz.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BudgetBlitz.Application.Services;

public class DataExportService(IUnitOfWork unitOfWork, UserManager<User> userManager) : IDataExportService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly UserManager<User> _userManager = userManager;

    public Task<byte[][]> ExportCSVAsync(byte[][] files)
    {
        List<byte[]> workBooks = [];

        foreach (byte[] file in files)
        {
            var workbook = new Workbook(new MemoryStream(file));

            byte[] workbookData = [];

            var opts = new TxtSaveOptions
            {
                Separator = '\t'
            };

            for (int idx = 0; idx < workbook.Worksheets.Count; idx++)
            {
                var ms = new MemoryStream();

                workbook.Worksheets.ActiveSheetIndex = idx;

                workbook.Save(ms, opts);

                ms.Position = 0;
                byte[] sheetData = ms.ToArray();

                byte[] combinedArray = new byte[workbookData.Length + sheetData.Length];
                Array.Copy(workbookData, 0, combinedArray, 0, workbookData.Length);
                Array.Copy(sheetData, 0, combinedArray, workbookData.Length, sheetData.Length);

                workbookData = combinedArray;
            }

            workBooks.Add(workbookData);
        }

        return Task.FromResult(workBooks.ToArray());
    }

    public async Task<byte[][]> ExportExcelAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(principal);
        if (user is null)
            return [];

        string nameOfSheet = user.UserName!;

        var userWithInfomation = await _unitOfWork.Users.GetAllInfoAsync(user.Id);

        List<Expense> expenses = [.. userWithInfomation.Expenses];
        List<Income> incomes = [.. userWithInfomation.Incomes];

        List<Task<byte[]>> tasks =
        [
            ExcelHelper.CreateFile($"{nameOfSheet} Expenses", expenses),
            ExcelHelper.CreateFile($"{nameOfSheet} Incmoes", incomes)
        ];

        return await Task.WhenAll(tasks);
    }
}
