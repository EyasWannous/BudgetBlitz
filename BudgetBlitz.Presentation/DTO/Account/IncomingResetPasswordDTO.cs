using System.ComponentModel.DataAnnotations;

namespace BudgetBlitz.Presentation.DTO.Account;

public class IncomingResetPasswordDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 5)]

    public string NewPasswrod { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required]
    public string Token { get; set; } = string.Empty;


    public bool EmailIsNullOrWhiteSpace
        => string.IsNullOrWhiteSpace(Email);
 
    public bool CheckPasswords => NewPasswrod != ConfirmPassword;

}
