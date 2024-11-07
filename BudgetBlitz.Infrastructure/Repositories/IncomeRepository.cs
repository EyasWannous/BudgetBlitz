using BudgetBlitz.Domain.Abstractions.IRepositories;
using BudgetBlitz.Domain.Models;
using BudgetBlitz.Infrastructure.Data;
using BudgetBlitz.Infrastructure.Services;

namespace BudgetBlitz.Infrastructure.Repositories;

public class IncomeRepository(AppDbContext context, ICacheService cacheService)
    : BaseRepository<Income>(context, cacheService), IIncomeRepository
{
}
