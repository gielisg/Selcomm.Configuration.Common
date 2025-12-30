namespace Selcomm.Configuration.Common.Models;

/// <summary>
/// Password reset configuration settings.
/// </summary>
public class PasswordResetSettings
{
    /// <summary>Password reset link expiration in minutes.</summary>
    public int ExpirationMinutes { get; set; } = 60;

    /// <summary>Base URL for password reset link.</summary>
    public string ResetLinkUrl { get; set; } = string.Empty;
}
