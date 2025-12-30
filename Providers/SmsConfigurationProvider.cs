using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Selcomm.Configuration.Common.Caching;
using Selcomm.Configuration.Common.Models;

namespace Selcomm.Configuration.Common.Providers;

/// <summary>
/// SMS configuration provider with domain-specific fallback.
/// </summary>
public class SmsConfigurationProvider : DomainConfigurationProvider<SmsSettings>
{
    /// <summary>
    /// Creates a new SMS configuration provider.
    /// </summary>
    public SmsConfigurationProvider(
        IConfiguration configuration,
        ILogger<SmsConfigurationProvider> logger,
        IConfigurationCache<SmsSettings>? cache = null)
        : base(configuration, "SmsSettings", "DomainSmsSettings", logger, cache)
    {
    }
}
