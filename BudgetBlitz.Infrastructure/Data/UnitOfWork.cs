using BudgetBlitz.Domain.Abstractions;
using BudgetBlitz.Domain.Abstractions.IRepositories;
using BudgetBlitz.Infrastructure.Repositories;
using BudgetBlitz.Infrastructure.Services;

namespace BudgetBlitz.Infrastructure.Data;

public class UnitOfWork(AppDbContext context, ICacheService cacheService) : IUnitOfWork
{
    private readonly AppDbContext _context = context;

    public IUserRepository Users { get; } = new UserRepository(context, cacheService);

    public IIncomeRepository Incomes { get; } = new IncomeRepository(context, cacheService);

    public ICategoryRepository Categories { get; } = new CategoryRepository(context, cacheService);

    public IExpenseRepository Expenses { get; } = new ExpenseRepository(context, cacheService);

    public IRefreshTokenRepository RefreshTokens { get; } = new RefreshTokenRepository(context, cacheService);

    public IUserDeviceRepository UserDevices { get; } = new UserDeviceRepository(context, cacheService);

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
