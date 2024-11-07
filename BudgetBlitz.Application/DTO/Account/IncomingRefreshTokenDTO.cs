using System.ComponentModel.DataAnnotations;

namespace BudgetBlitz.Application.DTO.Account;

public class IncomingRefreshTokenDTO
{
    [Required]
    public required string JwtToken { get; set; }
    
    [Required]
    public required string RefreshToken { get; set; }

    public bool JwtTokenOrRefreshTokenIsNullOrWhiteSpace
        => string.IsNullOrWhiteSpace(JwtToken) || string.IsNullOrWhiteSpace(RefreshToken);
}
