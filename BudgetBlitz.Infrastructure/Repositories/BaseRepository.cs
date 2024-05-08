using BudgetBlitz.Application.IServices;
using BudgetBlitz.Domain.Abstractions;
using BudgetBlitz.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BudgetBlitz.Infrastructure.Repositories;

public class BaseRepository<T>(AppDbContext context, ICacheService cacheService) : IBaseRepository<T> where T : class
{
    protected readonly AppDbContext _context = context;
    private readonly ICacheService _cacheService = cacheService;

    public async Task<T> AddAsync(T entity)
    {
        string keyAll = $"allOf_{typeof(T)}";
        string keyone = $"oneOf_{typeof(T)}";

        await _cacheService.RemoveAsync(keyAll);
        await _cacheService.RemoveByPrefixAsync(keyone);

        await _context.Set<T>().AddAsync(entity);
        
        return entity;
    }

    public async Task<int> CountAsync()
    {
        return await _context.Set<T>().CountAsync();
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> criteria)
    {
        return await _context.Set<T>().CountAsync(criteria);
    }

    public async Task DeleteAsync(T entity)
    {
        string keyAll = $"allOf_{typeof(T)}";
        string keyone = $"oneOf_{typeof(T)}";

        await _cacheService.RemoveAsync(keyAll);
        await _cacheService.RemoveByPrefixAsync(keyone);

        _context.Set<T>().Remove(entity);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        string key = $"allOf_{typeof(T)}";

        var items = await _cacheService.GetAsync<IEnumerable<T>>(key);
        if (items is not null)
            return items;

        items = await _context.Set<T>().AsNoTracking().ToListAsync();
        
        await _cacheService.SetAsync(key, items);

        return items;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        string key = $"oneOf_{typeof(T)}_{id}";
        
        var item = await _cacheService.GetAsync<T>(key);
        if (item is not null)
            return item;
        
        item = await _context.Set<T>().FindAsync(id);
        if (item is not null)
            await _cacheService.SetAsync(key, item);
        
        return item;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        string key = $"allOf_{typeof(T)}";
        await _cacheService.RemoveAsync(key);

        _context.Set<T>().Update(entity);

        return entity;
    }
}
