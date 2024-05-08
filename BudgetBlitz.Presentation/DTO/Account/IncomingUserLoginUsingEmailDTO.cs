using System.ComponentModel.DataAnnotations;

namespace BudgetBlitz.Presentation.DTO.Account;

public class IncomingUserLoginUsingEmailDTO
{
    [Required]
    [EmailAddress]
    [StringLength(50)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string Password { get; set; } = string.Empty;
}
