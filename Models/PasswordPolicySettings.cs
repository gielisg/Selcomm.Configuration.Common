namespace Selcomm.Configuration.Common.Models;

/// <summary>
/// Password policy configuration settings.
/// </summary>
public class PasswordPolicySettings
{
    /// <summary>Whether to prevent reusing old passwords.</summary>
    public bool PreventPasswordReuse { get; set; } = true;

    /// <summary>Number of previous passwords to track for reuse prevention.</summary>
    public int PasswordHistoryLimit { get; set; } = 5;

    /// <summary>Minimum password length.</summary>
    public int MinimumLength { get; set; } = 8;

    /// <summary>Maximum password length.</summary>
    public int MaximumLength { get; set; } = 128;

    /// <summary>Require at least one uppercase letter.</summary>
    public bool RequireUppercase { get; set; } = true;

    /// <summary>Require at least one lowercase letter.</summary>
    public bool RequireLowercase { get; set; } = true;

    /// <summary>Require at least one digit.</summary>
    public bool RequireDigit { get; set; } = true;

    /// <summary>Require at least one special character.</summary>
    public bool RequireSpecialCharacter { get; set; } = true;

    /// <summary>Allowed special characters.</summary>
    public string SpecialCharacters { get; set; } = "!@#$%^&*()_+-=[]{}|;:,.<>?";

    /// <summary>Password expiration in days (0 = never expires).</summary>
    public int PasswordExpirationDays { get; set; } = 90;

    /// <summary>Days before expiration to warn user.</summary>
    public int PasswordExpirationWarningDays { get; set; } = 14;
}
