using System.Security.Claims;

namespace BudgetBlitz.Application.IServices;

public interface IDataExportService
{
    Task<byte[][]> ExportExcelAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);

    Task<byte[][]> ExportCSVAsync(byte[][] files);
}
