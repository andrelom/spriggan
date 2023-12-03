using Microsoft.Extensions.Caching.Memory;

namespace Spriggan.Foundation.Caching;

internal class MemoryCache: IMemoryCache
{
    private readonly Microsoft.Extensions.Caching.Memory.IMemoryCache _cache;

    public MemoryCache(Microsoft.Extensions.Caching.Memory.IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<TItem?> Get<TItem>(string? key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return Task.FromResult<TItem?>(default);
        }

        return _cache.TryGetValue(key, out TItem? value)
            ? Task.FromResult(value)
            : Task.FromResult<TItem?>(default);
    }

    public Task<TItem> Set<TItem>(string? key, TItem value, TimeSpan expiration)
    {
        if (!string.IsNullOrWhiteSpace(key))
        {
            _cache.Set(key, value, expiration);
        }

        return Task.FromResult(value);
    }

    public Task<bool> Remove(string? key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return Task.FromResult(false);
        }

        _cache.Remove(key);

        return Task.FromResult(true);
    }
}
