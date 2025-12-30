namespace Selcomm.Configuration.Common.Caching;

/// <summary>
/// Interface for configuration caching.
/// </summary>
/// <typeparam name="T">Configuration type</typeparam>
public interface IConfigurationCache<T> where T : class
{
    /// <summary>
    /// Tries to get a cached value.
    /// </summary>
    /// <param name="key">Cache key</param>
    /// <param name="value">Cached value if found</param>
    /// <returns>True if value was found in cache</returns>
    bool TryGet(string key, out T? value);

    /// <summary>
    /// Sets a value in the cache.
    /// </summary>
    /// <param name="key">Cache key</param>
    /// <param name="value">Value to cache</param>
    /// <param name="expiration">Optional custom expiration time</param>
    void Set(string key, T value, TimeSpan? expiration = null);

    /// <summary>
    /// Removes a value from the cache.
    /// </summary>
    /// <param name="key">Cache key</param>
    void Remove(string key);

    /// <summary>
    /// Clears all cached values.
    /// </summary>
    void Clear();
}
