using BudgetBlitz.Domain.Abstractions.IRepositories;
using BudgetBlitz.Domain.Models;
using BudgetBlitz.Infrastructure.Data;
using BudgetBlitz.Infrastructure.Services;

namespace BudgetBlitz.Infrastructure.Repositories;

public class ExpenseRepository(AppDbContext context, ICacheService cacheService)
    : BaseRepository<Expense>(context, cacheService), IExpenseRepository
{
}
