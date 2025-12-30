namespace Selcomm.Configuration.Common.Models;

/// <summary>
/// Email confirmation configuration settings.
/// </summary>
public class EmailConfirmationSettings
{
    /// <summary>Email confirmation token expiration in hours.</summary>
    public int ExpirationHours { get; set; } = 24;

    /// <summary>Base URL for email confirmation link.</summary>
    public string ConfirmationLinkUrl { get; set; } = string.Empty;
}
