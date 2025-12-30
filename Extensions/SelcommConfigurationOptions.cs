namespace Selcomm.Configuration.Common.Extensions;

/// <summary>
/// Options for Selcomm configuration registration.
/// </summary>
public class SelcommConfigurationOptions
{
    /// <summary>
    /// Enable configuration caching (default: true).
    /// </summary>
    public bool EnableCaching { get; set; } = true;

    /// <summary>
    /// Cache expiration time (default: 5 minutes).
    /// </summary>
    public TimeSpan CacheExpiration { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Validate configuration on startup (default: true).
    /// </summary>
    public bool ValidateOnStartup { get; set; } = true;

    /// <summary>
    /// Fail application startup on validation errors (default: false).
    /// When false, errors are logged as warnings.
    /// </summary>
    public bool FailOnValidationErrors { get; set; } = false;

    /// <summary>
    /// Base path for security policy JSON files (default: "Configuration").
    /// </summary>
    public string SecurityPolicyBasePath { get; set; } = "Configuration";
}
