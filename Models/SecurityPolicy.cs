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
    public SecurityPolicyPasswordSettings PasswordPolicy { get; set; } = new();

    /// <summary>Email confirmation settings.</summary>
    public SecurityPolicyEmailConfirmationSettings EmailConfirmation { get; set; } = new();

    /// <summary>Mobile confirmation settings.</summary>
    public SecurityPolicyMobileConfirmationSettings MobileConfirmation { get; set; } = new();

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
    public int MaxFailedLoginAttempts { get; set; } = 5;

    /// <summary>Lockout duration in minutes.</summary>
    public int LockoutDurationMinutes { get; set; } = 30;

    /// <summary>Reset failed attempts after this many minutes of inactivity.</summary>
    public int ResetFailedAttemptsAfterMinutes { get; set; } = 60;

    /// <summary>Enable unknown device tracking.</summary>
    public bool TrackUnknownDevices { get; set; } = true;

    /// <summary>Require MFA for logins from new/unknown devices.</summary>
    public bool RequireMfaForNewDevices { get; set; } = false;
}

/// <summary>
/// Password policy settings within a security policy.
/// Uses a different name to avoid conflict with the domain configuration PasswordPolicySettings.
/// </summary>
public class SecurityPolicyPasswordSettings
{
    /// <summary>Minimum password length.</summary>
    public int MinimumLength { get; set; } = 8;

    /// <summary>Maximum password length.</summary>
    public int MaximumLength { get; set; } = 128;

    /// <summary>Require at least one uppercase letter.</summary>
    public bool RequireUppercase { get; set; } = true;

    /// <summary>Require at least one lowercase letter.</summary>
    public bool RequireLowercase { get; set; } = true;

    /// <summary>Require at least one digit.</summary>
    public bool RequireDigits { get; set; } = true;

    /// <summary>Require at least one special character.</summary>
    public bool RequireSpecialCharacters { get; set; } = true;

    /// <summary>Allowed special characters.</summary>
    public string SpecialCharacters { get; set; } = "!@#$%^&*()_+-=[]{}|;:,.<>?";

    /// <summary>Whether to prevent password reuse.</summary>
    public bool PreventPasswordReuse { get; set; } = true;

    /// <summary>Number of previous passwords to track for reuse prevention.</summary>
    public int PasswordHistoryCount { get; set; } = 5;

    /// <summary>Password expiration in days (0 = never expires).</summary>
    public int PasswordExpirationDays { get; set; } = 90;

    /// <summary>Days before expiration to warn user.</summary>
    public int PasswordExpirationWarningDays { get; set; } = 14;

    /// <summary>Check against common password blacklist.</summary>
    public bool CommonPasswordBlacklist { get; set; } = true;
}

/// <summary>
/// Email confirmation settings within a security policy.
/// Uses a different name to avoid conflict with the domain configuration EmailConfirmationSettings.
/// </summary>
public class SecurityPolicyEmailConfirmationSettings
{
    /// <summary>Whether email confirmation is enabled.</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>Token expiration in minutes.</summary>
    public int TokenExpirationMinutes { get; set; } = 1440;

    /// <summary>Maximum attempts per day.</summary>
    public int MaxAttemptsPerDay { get; set; } = 5;

    /// <summary>Minimum days between attempts.</summary>
    public int MinimumDaysBetweenAttempts { get; set; } = 0;

    /// <summary>Minimum hours between attempts.</summary>
    public int MinimumHoursBetweenAttempts { get; set; } = 1;

    /// <summary>Block after maximum attempts.</summary>
    public bool BlockAfterMaxAttempts { get; set; } = true;

    /// <summary>Block duration in hours.</summary>
    public int BlockDurationHours { get; set; } = 24;
}

/// <summary>
/// Mobile confirmation settings within a security policy.
/// Uses a different name to avoid conflict with the domain configuration MobileConfirmationSettings.
/// </summary>
public class SecurityPolicyMobileConfirmationSettings
{
    /// <summary>Whether mobile confirmation is enabled.</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>Code expiration in minutes.</summary>
    public int CodeExpirationMinutes { get; set; } = 10;

    /// <summary>Code length (number of digits).</summary>
    public int CodeLength { get; set; } = 6;

    /// <summary>Maximum attempts per day.</summary>
    public int MaxAttemptsPerDay { get; set; } = 5;

    /// <summary>Minimum days between attempts.</summary>
    public int MinimumDaysBetweenAttempts { get; set; } = 0;

    /// <summary>Minimum hours between attempts.</summary>
    public int MinimumHoursBetweenAttempts { get; set; } = 1;

    /// <summary>Block after maximum attempts.</summary>
    public bool BlockAfterMaxAttempts { get; set; } = true;

    /// <summary>Block duration in hours.</summary>
    public int BlockDurationHours { get; set; } = 24;
}

/// <summary>
/// Session management settings.
/// </summary>
public class SessionManagementSettings
{
    /// <summary>Access token expiration in minutes.</summary>
    public int AccessTokenExpirationMinutes { get; set; } = 15;

    /// <summary>Refresh token expiration in days.</summary>
    public int RefreshTokenExpirationDays { get; set; } = 30;

    /// <summary>Allow concurrent sessions.</summary>
    public bool AllowConcurrentSessions { get; set; } = true;

    /// <summary>Maximum concurrent sessions (0 = unlimited).</summary>
    public int MaxConcurrentSessions { get; set; } = 5;

    /// <summary>Invalidate all sessions on password change.</summary>
    public bool InvalidateTokensOnPasswordChange { get; set; } = true;

    /// <summary>Invalidate all sessions on MFA toggle.</summary>
    public bool InvalidateTokensOnMfaToggle { get; set; } = true;
}

/// <summary>
/// MFA policy settings.
/// </summary>
public class MfaPolicySettings
{
    /// <summary>Require MFA for all users.</summary>
    public bool Required { get; set; } = false;

    /// <summary>When true, at least one MFA method (primary or backup) must be active.</summary>
    public bool RequireMfa { get; set; } = false;

    /// <summary>Allowed MFA methods.</summary>
    public List<string> AllowedMethods { get; set; } = new() { "totp", "email", "sms" };

    /// <summary>Default MFA method.</summary>
    public string DefaultMethod { get; set; } = "totp";

    /// <summary>Grace period in days to set up MFA (0 = immediate).</summary>
    public int GracePeriodDays { get; set; } = 0;

    /// <summary>Days to remember trusted devices.</summary>
    public int RememberDeviceDays { get; set; } = 30;
}
