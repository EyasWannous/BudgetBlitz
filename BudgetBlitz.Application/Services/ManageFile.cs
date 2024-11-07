using BudgetBlitz.Application.Helper;
using BudgetBlitz.Application.IServices;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Microsoft.AspNetCore.StaticFiles;

namespace BudgetBlitz.Application.Services;

public class ManageFile : IManageFile
{
    public async Task<(byte[], string ContentType, string Path)> DownloadFileAsync(string fileName)
    {
        try
        {
            var getFilePath = DirectoryPathGetter.GetFilePath(fileName);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(getFilePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var allBytes = await File.ReadAllBytesAsync(getFilePath);

            return (allBytes, contentType, Path.GetFileName(getFilePath));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw new NotImplementedException();
        }
    }

    public async Task<string> UploadFileAsync(IFormFile formFile)
    {
        string fileName = "";

        try
        {
            var fileInfo = new FileInfo(formFile.Name);

            fileName = fileInfo.Name + '_' + DateTime.UtcNow.Ticks.ToString() + fileInfo.Extension;

            var getFilePath = DirectoryPathGetter.GetFilePath(fileName);

            using (var fs = new FileStream(getFilePath, FileMode.Create))
                await formFile.CopyToAsync(fs);

            return fileName;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw new NotImplementedException();
        }
    }
}
