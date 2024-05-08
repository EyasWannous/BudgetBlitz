namespace BudgetBlitz.Application.Helper;

public static class UnixConverter
{
    public static Task<DateTime> UnixTimeStampToDateTimeAsync(long unixDate)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, 0, DateTimeKind.Utc);

        dateTime = dateTime.AddSeconds(unixDate).ToUniversalTime();

        return Task.FromResult(dateTime);
    }
}
