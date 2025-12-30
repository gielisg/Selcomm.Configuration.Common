namespace Selcomm.Configuration.Common.Models;

/// <summary>
/// Mobile/SMS confirmation configuration settings.
/// </summary>
public class MobileConfirmationSettings
{
    /// <summary>Mobile confirmation code expiration in minutes.</summary>
    public int ExpirationMinutes { get; set; } = 10;

    /// <summary>Mobile confirmation code length (number of digits).</summary>
    public int CodeLength { get; set; } = 6;
}
