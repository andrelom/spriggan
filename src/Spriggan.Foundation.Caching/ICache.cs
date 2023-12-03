namespace Spriggan.Foundation.Caching;

public interface ICache
{
    Task<TItem?> Get<TItem>(string? key);

    Task<TItem> Set<TItem>(string? key, TItem value, TimeSpan expiration);

    Task<bool> Remove(string? key);
}
