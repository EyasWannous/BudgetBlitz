using NPOI.SS.UserModel;
using NPOI.Util.ArrayExtensions;
using NPOI.XSSF.UserModel;
using System.Reflection;

namespace BudgetBlitz.Application.Helper;

public static class ExcelHelper
{
    public static Task<byte[]> CreateFile<T>(string nameOfSheet ,List<T> source)
    {
        var workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet(nameOfSheet);
        var rowHeader = sheet.CreateRow(0);

        var properties = typeof(T).GetProperties();

        //header
        var font = workbook.CreateFont();
        font.IsBold = true;
        var style = workbook.CreateCellStyle();
        style.SetFont(font);

        var colIndex = 0;
        foreach (var property in properties)
        {
            var cell = rowHeader.CreateCell(colIndex);
            cell.SetCellValue(property.Name);
            cell.CellStyle = style;
            colIndex++;
        }
        //end header


        //content
        var rowNum = 1;
        foreach (var item in source)
        {
            var rowContent = sheet.CreateRow(rowNum);

            var colContentIndex = 0;

            foreach (var property in properties)
            {
                MakeCellContent(item, rowContent, colContentIndex, property);

                colContentIndex++;
            }

            rowNum++;
        }
        //end content


        var stream = new MemoryStream();
        workbook.Write(stream);
        var content = stream.ToArray();

        return Task.FromResult(content);
    }

    private static void MakeCellContent<T>(T? item, IRow rowContent, int colContentIndex, PropertyInfo property)
    {
        var cellContent = rowContent.CreateCell(colContentIndex);
        var value = property.GetValue(item, null);

        if (value == null)
        {
            cellContent.SetCellValue("");
            return;
        }
        if (property.PropertyType == typeof(string))
        {
            cellContent.SetCellValue(value.ToString());
            return;
        }
        if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
        {
            cellContent.SetCellValue(Convert.ToInt32(value));
            return;
        }
        if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
        {
            cellContent.SetCellValue(Convert.ToDouble(value));
            return;
        }
        if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
        {
            var dateValue = (DateTime)value;
            cellContent.SetCellValue(dateValue.ToString("yyyy-MM-dd"));
            return;
        }

        cellContent.SetCellValue(value.ToString());
    }
}
