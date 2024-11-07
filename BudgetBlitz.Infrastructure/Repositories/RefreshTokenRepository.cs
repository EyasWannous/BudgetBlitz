using BudgetBlitz.Infrastructure.Services;
using BudgetBlitz.Domain.Models;
using BudgetBlitz.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using BudgetBlitz.Domain.Abstractions.IRepositories;

namespace BudgetBlitz.Infrastructure.Repositories;

public class RefreshTokenRepository(AppDbContext context, ICacheService cacheService)
    : BaseRepository<RefreshToken>(context, cacheService), IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetByTokenAsync(string token)
        => await _context.RefreshTokens.FirstOrDefaultAsync(item => item.Token == token);

    public async Task<bool> MakeRefreshTokenAsUsedAsync(RefreshToken refreshToken)
    {
        var userRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(item => item.Token == refreshToken.Token);
        if (userRefreshToken is null)
            return false;

        userRefreshToken.IsUsed = true;
        return true;
    }
}
