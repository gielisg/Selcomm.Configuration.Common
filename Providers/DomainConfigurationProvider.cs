using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Selcomm.Configuration.Common.Abstractions;
using Selcomm.Configuration.Common.Caching;

namespace Selcomm.Configuration.Common.Providers;

/// <summary>
/// Base implementation for domain-aware configuration providers.
/// </summary>
/// <typeparam name="T">Configuration type</typeparam>
public class DomainConfigurationProvider<T> : IDomainConfigurationProvider<T>
    where T : class, new()
{
    private readonly IConfiguration _configuration;
    private readonly IConfigurationCache<T>? _cache;
    private readonly string _globalSectionName;
    private readonly string _domainSectionName;
    private readonly ILogger _logger;

    /// <summary>
    /// Creates a new domain configuration provider.
    /// </summary>
    /// <param name="configuration">Application configuration</param>
    /// <param name="globalSectionName">Name of the global configuration section</param>
    /// <param name="domainSectionName">Name of the domain-specific configuration section prefix</param>
    /// <param name="logger">Logger instance</param>
    /// <param name="cache">Optional configuration cache</param>
    public DomainConfigurationProvider(
        IConfiguration configuration,
        string globalSectionName,
        string domainSectionName,
        ILogger logger,
        IConfigurationCache<T>? cache = null)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _globalSectionName = globalSectionName ?? throw new ArgumentNullException(nameof(globalSectionName));
        _domainSectionName = domainSectionName ?? throw new ArgumentNullException(nameof(domainSectionName));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cache = cache;
    }

    /// <inheritdoc/>
    public T GetSettings(string domain)
    {
        var cacheKey = GetCacheKey(domain);

        // Try cache first
        if (_cache?.TryGet(cacheKey, out var cached) == true && cached != null)
        {
            return cached;
        }

        // Try domain-specific settings
        var domainSettings = GetDomainSettings(domain);
        if (domainSettings != null)
        {
            _cache?.Set(cacheKey, domainSettings);
            return domainSettings;
        }

        // Fall back to global
        var globalSettings = GetGlobalSettings();
        _logger.LogDebug("Using global {ConfigType} settings for domain {Domain}",
            typeof(T).Name, domain);

        // Cache with domain key to avoid repeated lookups
        _cache?.Set(cacheKey, globalSettings);
        return globalSettings;
    }

    /// <inheritdoc/>
    public T? GetDomainSettings(string domain)
    {
        if (string.IsNullOrEmpty(domain))
            return null;

        var section = _configuration.GetSection($"{_domainSectionName}:{domain}");
        if (!section.Exists())
            return null;

        var settings = new T();
        section.Bind(settings);

        _logger.LogDebug("Loaded domain-specific {ConfigType} settings for {Domain}",
            typeof(T).Name, domain);

        return settings;
    }

    /// <inheritdoc/>
    public T GetGlobalSettings()
    {
        var cacheKey = GetCacheKey("__global__");

        // Try cache first
        if (_cache?.TryGet(cacheKey, out var cached) == true && cached != null)
        {
            return cached;
        }

        var settings = new T();
        var section = _configuration.GetSection(_globalSectionName);
        section.Bind(settings);

        _cache?.Set(cacheKey, settings);
        return settings;
    }

    /// <inheritdoc/>
    public IReadOnlyList<string> GetConfiguredDomains()
    {
        var section = _configuration.GetSection(_domainSectionName);
        return section.GetChildren().Select(c => c.Key).ToList();
    }

    /// <inheritdoc/>
    public void Refresh()
    {
        _cache?.Clear();
        _logger.LogInformation("Refreshed all {ConfigType} configuration cache", typeof(T).Name);
    }

    /// <inheritdoc/>
    public void Refresh(string domain)
    {
        _cache?.Remove(GetCacheKey(domain));
        _logger.LogInformation("Refreshed {ConfigType} configuration cache for domain {Domain}",
            typeof(T).Name, domain);
    }

    private string GetCacheKey(string domain) => $"{typeof(T).Name}:{domain}";
}
