using System.Collections.Concurrent;

namespace Selcomm.Configuration.Common.Caching;

/// <summary>
/// In-memory configuration cache with expiration support.
/// </summary>
/// <typeparam name="T">Configuration type</typeparam>
public class ConfigurationCache<T> : IConfigurationCache<T> where T : class
{
    private readonly ConcurrentDictionary<string, CacheEntry> _cache = new();
    private readonly TimeSpan _defaultExpiration;

    /// <summary>
    /// Creates a new configuration cache.
    /// </summary>
    /// <param name="defaultExpiration">Default expiration time (default: 5 minutes)</param>
    public ConfigurationCache(TimeSpan? defaultExpiration = null)
    {
        _defaultExpiration = defaultExpiration ?? TimeSpan.FromMinutes(5);
    }

    /// <inheritdoc/>
    public bool TryGet(string key, out T? value)
    {
        if (_cache.TryGetValue(key, out var entry))
        {
            if (entry.ExpiresAt > DateTime.UtcNow)
            {
                value = entry.Value;
                return true;
            }
            // Entry expired, remove it
            _cache.TryRemove(key, out _);
        }
        value = null;
        return false;
    }

    /// <inheritdoc/>
    public void Set(string key, T value, TimeSpan? expiration = null)
    {
        var exp = expiration ?? _defaultExpiration;
        _cache[key] = new CacheEntry(value, DateTime.UtcNow.Add(exp));
    }

    /// <inheritdoc/>
    public void Remove(string key)
    {
        _cache.TryRemove(key, out _);
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _cache.Clear();
    }

    private record CacheEntry(T Value, DateTime ExpiresAt);
}
