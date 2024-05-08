namespace BudgetBlitz.Application.Options;

public class JwtOptions
{
    public string SigningKey { get; set; } = string.Empty;
    public TimeSpan ExpireTime { get; set; }
    public string ValidIssuer { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty;
}
