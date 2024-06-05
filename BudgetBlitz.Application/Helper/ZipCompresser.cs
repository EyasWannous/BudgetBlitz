using System.IO.Compression;

namespace BudgetBlitz.Application.Helper;

public static class ZipCompresser
{
    public static async Task<byte[]> CompressFiles(byte[][] files, string extension, CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            for (int i = 0; i < files.Length; i++)
            {
                byte[] file = files[i];

                var entry = archive.CreateEntry($"File{i}.{extension}", CompressionLevel.Optimal);

                using var entryStream = entry.Open();
                // First way
                //using var fileStream = new MemoryStream(file);

                //await fileStream.CopyToAsync(entryStream, cancellationToken);

                // Second way
                await entryStream.WriteAsync(file, 0, file.Length, cancellationToken);
            }
        }

        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream.ToArray();
    }
}
