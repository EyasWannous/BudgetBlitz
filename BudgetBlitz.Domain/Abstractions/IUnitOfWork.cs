using BudgetBlitz.Domain.Models;

namespace BudgetBlitz.Domain.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IIncomeRepository Incomes { get; }
    ICategoryRepository Categories { get; }
    IExpenseRepository Expenses { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    Task<int> CompleteAsync();
}
