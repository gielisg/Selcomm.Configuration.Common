using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Selcomm.Configuration.Common.Caching;
using Selcomm.Configuration.Common.Models;

namespace Selcomm.Configuration.Common.Providers;

/// <summary>
/// JWT configuration provider with domain-specific fallback.
/// </summary>
public class JwtConfigurationProvider : DomainConfigurationProvider<JwtSettings>
{
    /// <summary>
    /// Creates a new JWT configuration provider.
    /// </summary>
    public JwtConfigurationProvider(
        IConfiguration configuration,
        ILogger<JwtConfigurationProvider> logger,
        IConfigurationCache<JwtSettings>? cache = null)
        : base(configuration, "JwtSettings", "DomainJwtSettings", logger, cache)
    {
    }
}
