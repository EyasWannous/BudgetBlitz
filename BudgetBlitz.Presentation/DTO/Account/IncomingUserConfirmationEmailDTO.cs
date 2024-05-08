using System.ComponentModel.DataAnnotations;

namespace BudgetBlitz.Presentation.DTO.Account;

public class IncomingUserConfirmationEmailDTO
{
    [Required]
    public string UserId { get; set; } = string.Empty;
    [Required]
    public string Token { get; set; } = string.Empty;

    
    public bool UserIdOrTokenIsNullOrWhiteSpace
        => string.IsNullOrWhiteSpace(UserId) || string.IsNullOrWhiteSpace(Token);
}
