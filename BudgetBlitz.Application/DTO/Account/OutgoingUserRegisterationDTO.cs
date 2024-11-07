namespace BudgetBlitz.Application.DTO.Account;

public class OutgoingUserRegisterationDTO
{
    public string Message { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public IEnumerable<string> Errors { get; set; } = [];
    public DateTime? ExpireDate { get; set; }
    public string? JwtToken { get; set; }
    public string? RefreshToken { get; set; }
}
