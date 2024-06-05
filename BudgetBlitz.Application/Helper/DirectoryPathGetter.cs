namespace BudgetBlitz.Application.Helper;

public static class DirectoryPathGetter
{
    public static string GetCurrentDirecotry()
        => Directory.GetCurrentDirectory();

    public static string GetStaticContentDirectory()
    {
        var result = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\StaticContent\\");
        if(!Directory.Exists(result))
            Directory.CreateDirectory(result);

        return result;
    }

    public static string GetFilePath(string fileName)
    {
        var getStaticContentDirectory = GetStaticContentDirectory();

        return Path.Combine(getStaticContentDirectory, fileName);
    }
}
