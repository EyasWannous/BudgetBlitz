using BudgetBlitz.Application.IServices;
using BudgetBlitz.Infrastructure.Services;
using BudgetBlitz.Domain.Abstractions;
using BudgetBlitz.Domain.Models;
using BudgetBlitz.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BudgetBlitz.Infrastructure.Repositories;

public class UserRepository(AppDbContext context, ICacheService cacheService)
    : BaseRepository<User>(context, cacheService), IUserRepository
{
    public async Task<User> GetAllInfoAsync(int id)
        => await _context.Users
            .Where(user => user.Id == id)
            .Include(user => user.Incomes)
            .Include(user => user.Expenses)
            .SingleAsync();
}
