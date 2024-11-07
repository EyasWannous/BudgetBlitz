namespace BudgetBlitz.Application.DTO.Account;

public class OutgoingRefreshTokenDTO
{
    public string Message { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public string? JwtToken { get; set; }
    public string? RefreshToken { get; set; }
}
