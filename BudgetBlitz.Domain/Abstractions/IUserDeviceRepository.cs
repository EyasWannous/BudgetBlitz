using BudgetBlitz.Domain.Models;

namespace BudgetBlitz.Domain.Abstractions;

public interface IUserDeviceRepository : IBaseRepository<UserDevice>
{
    Task<bool> IsExistsAsync(string deviceToken);
}
