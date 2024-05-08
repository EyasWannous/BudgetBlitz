using System.ComponentModel.DataAnnotations;

namespace BudgetBlitz.Presentation.DTO.Account;

public class IncomingUserRegisterationDTO
{
    [Required]
    [EmailAddress]
    [StringLength(50)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 5)] 
    public string ConfirmPassword { get; set; } = string.Empty;

    public bool EmailIsNullOrWhiteSpace
     => string.IsNullOrWhiteSpace(Email);

    public bool CheckPasswords => Password != ConfirmPassword;
}
