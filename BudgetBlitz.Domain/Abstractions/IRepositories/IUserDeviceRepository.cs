using BudgetBlitz.Domain.Models;

namespace BudgetBlitz.Domain.Abstractions.IRepositories;

public interface IUserDeviceRepository : IBaseRepository<UserDevice>
{
    Task<bool> IsExistsAsync(string deviceToken);
}
