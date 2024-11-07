using BudgetBlitz.Domain.Abstractions.IRepositories;
using BudgetBlitz.Infrastructure.Data;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace BudgetBlitz.Infrastructure.Repositories;

// IMemoryCache memoryCache
public class CacheBaseRepository<T>(AppDbContext context, IBaseRepository<T> baseRepository, IDistributedCache distributedCache) : IBaseRepository<T> where T : class
{
    private readonly IBaseRepository<T> _baseRepository = baseRepository;
    //private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly IDistributedCache _distributedCache = distributedCache;
    private readonly AppDbContext _context = context;

    public Task<T> AddAsync(T entity) => _baseRepository.AddAsync(entity);

    public Task<int> CountAsync() => _baseRepository.CountAsync();

    public Task<int> CountAsync(Expression<Func<T, bool>> criteria)
        => _baseRepository.CountAsync(criteria);

    public Task DeleteAsync(T entity) => _baseRepository.DeleteAsync(entity);

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        string key = $"member-{typeof(T)}-all";

        string? cacheItems = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cacheItems))
            return JsonConvert.DeserializeObject<IEnumerable<T>>(cacheItems) ?? [];

        var items = await _baseRepository.GetAllAsync();

        await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(items));

        return items;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        string key = $"member-{typeof(T)}-{id}";

        string? cacheItem = await _distributedCache.GetStringAsync(key);

        if (!string.IsNullOrEmpty(cacheItem))
        {
            var model = JsonConvert.DeserializeObject<T>(cacheItem);
            if (model is null)
                return null;

            _context.Set<T>().Attach(model);

            return model;
        }

        var item = await _baseRepository.GetByIdAsync(id);
        if (item is null)
            return null;

        await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(item));

        return item;
    }

    //public Task<IEnumerable<T>> GetAllAsync()
    //{
    //    string key = $"member-{typeof(T)}-all";

    //    return _memoryCache.GetOrCreateAsync(
    //        key,
    //        entry =>
    //        {
    //            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

    //            return _baseRepository.GetAllAsync();
    //        });
    //}

    //public Task<T?> GetByIdAsync(int id)
    //{
    //    string key = $"member-{typeof(T)}-{id}";

    //    return _memoryCache.GetOrCreateAsync(
    //        key,
    //        entry =>
    //        {
    //            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

    //            return _baseRepository.GetByIdAsync(id);
    //        });
    //}

    public Task<T> UpdateAsync(T entity) => _baseRepository.UpdateAsync(entity);

}

