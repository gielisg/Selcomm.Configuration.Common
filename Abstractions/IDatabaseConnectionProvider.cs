namespace Selcomm.Configuration.Common.Abstractions;

/// <summary>
/// Provides database connection strings for domains.
/// </summary>
public interface IDatabaseConnectionProvider
{
    /// <summary>
    /// Gets the connection string for the specified domain.
    /// </summary>
    /// <param name="domain">Domain identifier</param>
    /// <returns>Connection string or null if not configured</returns>
    string? GetConnectionString(string domain);

    /// <summary>
    /// Checks if a connection string exists for the specified domain.
    /// </summary>
    /// <param name="domain">Domain identifier</param>
    /// <returns>True if connection string is configured</returns>
    bool HasConnectionString(string domain);

    /// <summary>
    /// Gets all configured domains.
    /// </summary>
    /// <returns>List of domain identifiers that have connection strings</returns>
    IReadOnlyList<string> GetConfiguredDomains();
}
