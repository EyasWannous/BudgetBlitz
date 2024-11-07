using BudgetBlitz.Domain.Abstractions.IRepositories;
using BudgetBlitz.Domain.Models;
using BudgetBlitz.Infrastructure.Data;
using BudgetBlitz.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace BudgetBlitz.Infrastructure.Repositories;

public class UserDeviceRepository(AppDbContext context, ICacheService cacheService)
    : BaseRepository<UserDevice>(context, cacheService), IUserDeviceRepository
{
    public async Task<bool> IsExistsAsync(string deviceToken)
        => await _context.UserDevices.AnyAsync(userDevicce => userDevicce.DeviceToken == deviceToken);
}
