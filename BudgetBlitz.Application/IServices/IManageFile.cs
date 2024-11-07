using Microsoft.AspNetCore.Http;

namespace BudgetBlitz.Application.IServices;

public interface IManageFile
{
    Task<String> UploadFileAsync(IFormFile formFile);
    Task<(byte[], string ContentType, string Path)> DownloadFileAsync(string fileName);
}
