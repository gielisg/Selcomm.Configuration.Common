namespace Selcomm.Configuration.Common.Abstractions;

/// <summary>
/// Provides domain-aware configuration with automatic fallback to global settings.
/// </summary>
/// <typeparam name="T">Configuration type</typeparam>
public interface IDomainConfigurationProvider<T> where T : class, new()
{
    /// <summary>
    /// Gets configuration for the specified domain, falling back to global if not found.
    /// </summary>
    /// <param name="domain">Domain identifier</param>
    /// <returns>Configuration settings (domain-specific or global fallback)</returns>
    T GetSettings(string domain);

    /// <summary>
    /// Gets configuration for the specified domain only.
    /// Does not fall back to global settings.
    /// </summary>
    /// <param name="domain">Domain identifier</param>
    /// <returns>Domain-specific configuration or null if not found</returns>
    T? GetDomainSettings(string domain);

    /// <summary>
    /// Gets the global (default) configuration.
    /// </summary>
    /// <returns>Global configuration settings</returns>
    T GetGlobalSettings();

    /// <summary>
    /// Gets all configured domains.
    /// </summary>
    /// <returns>List of domain identifiers that have specific configuration</returns>
    IReadOnlyList<string> GetConfiguredDomains();

    /// <summary>
    /// Refreshes cached configuration (if caching is enabled).
    /// </summary>
    void Refresh();

    /// <summary>
    /// Refreshes cached configuration for a specific domain.
    /// </summary>
    /// <param name="domain">Domain identifier</param>
    void Refresh(string domain);
}
