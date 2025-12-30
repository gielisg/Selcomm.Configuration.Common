namespace Selcomm.Configuration.Common.Models;

/// <summary>
/// Complete security policy for a domain.
/// </summary>
public class SecurityPolicy
{
    /// <summary>Domain identifier.</summary>
    public string Domain { get; set; } = string.Empty;

    /// <summary>Policy description.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Login security settings.</summary>
    public LoginSecuritySettings LoginSecurity { get; set; } = new();

    /// <summary>Password policy settings.</summary>
    public PasswordPolicySettings PasswordPolicy { get; set; } = new();

    /// <summary>Email confirmation settings.</summary>
    public EmailConfirmationPolicySettings EmailConfirmation { get; set; } = new();

    /// <summary>Mobile confirmation settings.</summary>
    public MobileConfirmationPolicySettings MobileConfirmation { get; set; } = new();

    /// <summary>Session management settings.</summary>
    public SessionManagementSettings SessionManagement { get; set; } = new();

    /// <summary>MFA policy settings.</summary>
    public MfaPolicySettings MfaPolicy { get; set; } = new();
}

/// <summary>
/// Login security settings within a security policy.
/// </summary>
public class LoginSecuritySettings
{
    /// <summary>Maximum failed login attempts before lockout.</summary>
    public int MaxFailedAttempts { get; set; } = 5;

    /// <summary>Lockout duration in minutes.</summary>
    public int LockoutDurationMinutes { get; set; } = 15;

    /// <summary>Reset failed attempts after this many minutes of inactivity.</summary>
    public int ResetFailedAttemptsAfterMinutes { get; set; } = 30;

    /// <summary>Enable device tracking.</summary>
    public bool EnableDeviceTracking { get; set; } = true;

    /// <summary>Send notifications for new device logins.</summary>
    public bool NotifyOnNewDevice { get; set; } = true;

    /// <summary>Require MFA for logins from new/unknown devices.</summary>
    public bool RequireMfaForNewDevice { get; set; } = false;

    /// <summary>Block login from new device if user has no MFA configured.</summary>
    public bool BlockNewDeviceWithoutMfa { get; set; } = false;
}

/// <summary>
/// Email confirmation policy settings.
/// </summary>
public class EmailConfirmationPolicySettings
{
    /// <summary>Require email confirmation for new accounts.</summary>
    public bool Required { get; set; } = false;

    /// <summary>Token expiration in hours.</summary>
    public int TokenExpirationHours { get; set; } = 24;

    /// <summary>Maximum confirmation emails per hour.</summary>
    public int MaxEmailsPerHour { get; set; } = 3;

    /// <summary>Maximum confirmation emails per day.</summary>
    public int MaxEmailsPerDay { get; set; } = 10;
}

/// <summary>
/// Mobile confirmation policy settings.
/// </summary>
public class MobileConfirmationPolicySettings
{
    /// <summary>Require mobile confirmation for new accounts.</summary>
    public bool Required { get; set; } = false;

    /// <summary>Code expiration in minutes.</summary>
    public int CodeExpirationMinutes { get; set; } = 10;

    /// <summary>Maximum SMS per hour.</summary>
    public int MaxSmsPerHour { get; set; } = 3;

    /// <summary>Maximum SMS per day.</summary>
    public int MaxSmsPerDay { get; set; } = 10;

    /// <summary>Code length (number of digits).</summary>
    public int CodeLength { get; set; } = 6;
}

/// <summary>
/// Session management settings.
/// </summary>
public class SessionManagementSettings
{
    /// <summary>Access token lifetime in minutes.</summary>
    public int AccessTokenLifetimeMinutes { get; set; } = 15;

    /// <summary>Refresh token lifetime in days.</summary>
    public int RefreshTokenLifetimeDays { get; set; } = 7;

    /// <summary>Allow concurrent sessions.</summary>
    public bool AllowConcurrentSessions { get; set; } = true;

    /// <summary>Maximum concurrent sessions (0 = unlimited).</summary>
    public int MaxConcurrentSessions { get; set; } = 0;

    /// <summary>Rotate refresh tokens on use.</summary>
    public bool RotateRefreshToken { get; set; } = false;

    /// <summary>Invalidate all sessions on password change.</summary>
    public bool InvalidateSessionsOnPasswordChange { get; set; } = true;
}

/// <summary>
/// MFA policy settings.
/// </summary>
public class MfaPolicySettings
{
    /// <summary>Require MFA for all users.</summary>
    public bool Required { get; set; } = false;

    /// <summary>Allowed MFA methods.</summary>
    public List<string> AllowedMethods { get; set; } = new() { "totp", "sms", "email" };

    /// <summary>Preferred MFA method.</summary>
    public string PreferredMethod { get; set; } = "totp";

    /// <summary>Allow backup MFA methods.</summary>
    public bool AllowBackupMethod { get; set; } = true;

    /// <summary>Grace period in days to set up MFA (0 = immediate).</summary>
    public int GracePeriodDays { get; set; } = 0;

    /// <summary>Days to remember trusted devices.</summary>
    public int RememberDeviceDays { get; set; } = 30;
}
