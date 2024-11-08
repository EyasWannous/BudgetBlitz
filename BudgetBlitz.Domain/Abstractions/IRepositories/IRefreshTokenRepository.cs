﻿using BudgetBlitz.Domain.Models;

namespace BudgetBlitz.Domain.Abstractions.IRepositories;

public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task<bool> MakeRefreshTokenAsUsedAsync(RefreshToken refreshToken);
}
