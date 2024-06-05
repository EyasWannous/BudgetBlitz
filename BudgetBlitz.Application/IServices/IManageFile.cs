using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;

namespace BudgetBlitz.Application.IServices;

public interface IManageFile
{
    Task<String> UploadFileAsync(IFormFile formFile);
    Task<(byte[], string ContentType, string Path)> DownloadFileAsync(string fileName);
}
