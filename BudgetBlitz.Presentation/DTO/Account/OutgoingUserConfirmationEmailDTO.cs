namespace BudgetBlitz.Presentation.DTO.Account;

public class OutgoingUserConfirmationEmailDTO
{
    public string Message { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public IEnumerable<string> Errors { get; set; } = [];
}
