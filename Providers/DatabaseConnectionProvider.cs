using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Selcomm.Configuration.Common.Abstractions;

namespace Selcomm.Configuration.Common.Providers;

/// <summary>
/// Provides database connection strings for domains.
/// </summary>
public class DatabaseConnectionProvider : IDatabaseConnectionProvider
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseConnectionProvider> _logger;
    private readonly string _sectionName;

    /// <summary>
    /// Creates a new database connection provider.
    /// </summary>
    public DatabaseConnectionProvider(
        IConfiguration configuration,
        ILogger<DatabaseConnectionProvider> logger,
        string sectionName = "DomainConnectionStrings")
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _sectionName = sectionName;
    }

    /// <inheritdoc/>
    public string? GetConnectionString(string domain)
    {
        if (string.IsNullOrEmpty(domain))
        {
            _logger.LogWarning("Attempted to get connection string with null or empty domain");
            return null;
        }

        var connectionString = _configuration[$"{_sectionName}:{domain}"];

        if (string.IsNullOrEmpty(connectionString))
        {
            _logger.LogWarning("No connection string configured for domain {Domain}", domain);
            return null;
        }

        return connectionString;
    }

    /// <inheritdoc/>
    public bool HasConnectionString(string domain)
    {
        if (string.IsNullOrEmpty(domain))
            return false;

        var connectionString = _configuration[$"{_sectionName}:{domain}"];
        return !string.IsNullOrEmpty(connectionString);
    }

    /// <inheritdoc/>
    public IReadOnlyList<string> GetConfiguredDomains()
    {
        var section = _configuration.GetSection(_sectionName);
        return section.GetChildren().Select(c => c.Key).ToList();
    }
}
