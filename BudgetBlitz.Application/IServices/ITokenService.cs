using BudgetBlitz.Domain.Models;

namespace BudgetBlitz.Application.IServices;

public interface ITokenService
{
    Task<(string JwtToken, string RefreshToken, DateTime ExpireDate)> GenerateJwtTokenAsync(User user);
    Task<(string Message, bool IsSuccess)> VerfiyTokenAsync(string jwtToken, string refreshToken);
    Task<(string Message, string? JwtToken, string? RefreshToken, bool IsSuccess)> MakeNewRefreshTokenAsync(string refreshToken);
}
