using System.ComponentModel.DataAnnotations;

namespace BudgetBlitz.Presentation.DTO.Account;

public class IncomingForgetPasswordDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    
    public bool EmailIsNullOrWhiteSpace
        => string.IsNullOrWhiteSpace(Email);
}
