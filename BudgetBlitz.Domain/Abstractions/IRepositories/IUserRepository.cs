using BudgetBlitz.Domain.Models;

namespace BudgetBlitz.Domain.Abstractions.IRepositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetAllInfoAsync(int id);
}
