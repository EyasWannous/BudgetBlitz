using System.ComponentModel.DataAnnotations;

namespace BudgetBlitz.Application.DTO.Account;

public class IncomingForgetPasswordDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    
    public bool EmailIsNullOrWhiteSpace
        => string.IsNullOrWhiteSpace(Email);
}
