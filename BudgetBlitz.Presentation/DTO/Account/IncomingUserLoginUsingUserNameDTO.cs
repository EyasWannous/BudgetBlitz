using System.ComponentModel.DataAnnotations;

namespace BudgetBlitz.Presentation.DTO.Account;

public class IncomingUserLoginUsingUserNameDTO
{
    [Required]
    [StringLength(50)]
    public string UserName { get; set; } = string.Empty;
    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string Password { get; set; } = string.Empty;

}
