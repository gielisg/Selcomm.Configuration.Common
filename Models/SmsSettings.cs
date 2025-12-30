namespace Selcomm.Configuration.Common.Models;

/// <summary>
/// SMS/Twilio configuration settings.
/// </summary>
public class SmsSettings
{
    /// <summary>SMS provider type (e.g., "Twilio").</summary>
    public string Provider { get; set; } = "Twilio";

    /// <summary>Twilio Account SID.</summary>
    public string TwilioAccountSid { get; set; } = string.Empty;

    /// <summary>Twilio Auth Token.</summary>
    public string TwilioAuthToken { get; set; } = string.Empty;

    /// <summary>Twilio sender phone number.</summary>
    public string TwilioPhoneNumber { get; set; } = string.Empty;

    // Branding

    /// <summary>Company name for SMS branding.</summary>
    public string? CompanyName { get; set; }

    /// <summary>Support email address displayed in SMS messages.</summary>
    public string? SupportEmail { get; set; }

    /// <summary>Domain-specific template folder name.</summary>
    public string? TemplateFolder { get; set; }
}
