using BudgetBlitz.Application.IServices;
using BudgetBlitz.Domain.Abstractions;
using BudgetBlitz.Domain.Models;
using BudgetBlitz.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;

namespace BudgetBlitz.Infrastructure.Repositories;

public class IncomeRepository(AppDbContext context, ICacheService cacheService) 
    : BaseRepository<Income>(context, cacheService), IIncomeRepository
{
}
