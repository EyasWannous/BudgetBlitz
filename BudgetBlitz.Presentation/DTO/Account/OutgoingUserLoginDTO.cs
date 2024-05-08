using BudgetBlitz.Domain.Models;

namespace BudgetBlitz.Presentation.DTO.Account;

public class OutgoingUserLoginDTO
{
    public string Message { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public DateTime? ExpireDate { get; set; }
    public string? JwtToken { get; set; }
    public string? RefreshToken { get; set; }
}
