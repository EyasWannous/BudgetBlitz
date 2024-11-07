using BudgetBlitz.Domain.Abstractions.IRepositories;
using BudgetBlitz.Domain.Models;
using BudgetBlitz.Infrastructure.Data;
using BudgetBlitz.Infrastructure.Services;

namespace BudgetBlitz.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext context, ICacheService cacheService)
    : BaseRepository<Category>(context, cacheService), ICategoryRepository
{
}
