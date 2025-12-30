using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Selcomm.Configuration.Common.Caching;
using Selcomm.Configuration.Common.Models;

namespace Selcomm.Configuration.Common.Providers;

/// <summary>
/// Password policy provider with domain-specific fallback.
/// </summary>
public class PasswordPolicyProvider : DomainConfigurationProvider<PasswordPolicySettings>
{
    /// <summary>
    /// Creates a new password policy provider.
    /// </summary>
    public PasswordPolicyProvider(
        IConfiguration configuration,
        ILogger<PasswordPolicyProvider> logger,
        IConfigurationCache<PasswordPolicySettings>? cache = null)
        : base(configuration, "PasswordPolicy", "DomainPasswordPolicy", logger, cache)
    {
    }
}
