using BudgetBlitz.Application.IServices;
using BudgetBlitz.Infrastructure.Services;
using BudgetBlitz.Domain.Abstractions;
using BudgetBlitz.Domain.Models;
using BudgetBlitz.Infrastructure.Data;

namespace BudgetBlitz.Infrastructure.Repositories;

public class UserRepository(AppDbContext context, ICacheService cacheService) 
    : BaseRepository<User>(context, cacheService), IUserRepository
{
}
