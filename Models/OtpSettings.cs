namespace Selcomm.Configuration.Common.Models;

/// <summary>
/// OTP/verification code configuration settings.
/// </summary>
public class OtpSettings
{
    /// <summary>OTP code expiration in minutes.</summary>
    public int ExpirationMinutes { get; set; } = 10;

    /// <summary>OTP code length (number of digits).</summary>
    public int CodeLength { get; set; } = 6;
}
