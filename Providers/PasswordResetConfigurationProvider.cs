using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Selcomm.Configuration.Common.Caching;
using Selcomm.Configuration.Common.Models;

namespace Selcomm.Configuration.Common.Providers;

/// <summary>
/// Password reset configuration provider with domain-specific fallback.
/// </summary>
public class PasswordResetConfigurationProvider : DomainConfigurationProvider<PasswordResetSettings>
{
    /// <summary>
    /// Creates a new password reset configuration provider.
    /// </summary>
    public PasswordResetConfigurationProvider(
        IConfiguration configuration,
        ILogger<PasswordResetConfigurationProvider> logger,
        IConfigurationCache<PasswordResetSettings>? cache = null)
        : base(configuration, "PasswordResetSettings", "DomainPasswordResetSettings", logger, cache)
    {
    }
}
