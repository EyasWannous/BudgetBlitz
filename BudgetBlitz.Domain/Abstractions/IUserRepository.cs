﻿using BudgetBlitz.Domain.Models;

namespace BudgetBlitz.Domain.Abstractions;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetAllInfoAsync(int id);
}
