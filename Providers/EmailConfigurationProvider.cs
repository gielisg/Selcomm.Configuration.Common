using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Selcomm.Configuration.Common.Caching;
using Selcomm.Configuration.Common.Models;

namespace Selcomm.Configuration.Common.Providers;

/// <summary>
/// Email configuration provider with domain-specific fallback.
/// </summary>
public class EmailConfigurationProvider : DomainConfigurationProvider<EmailSettings>
{
    /// <summary>
    /// Creates a new email configuration provider.
    /// </summary>
    public EmailConfigurationProvider(
        IConfiguration configuration,
        ILogger<EmailConfigurationProvider> logger,
        IConfigurationCache<EmailSettings>? cache = null)
        : base(configuration, "EmailSettings", "DomainEmailSettings", logger, cache)
    {
    }
}
