using Microsoft.Extensions.Configuration;

namespace Selcomm.Configuration.Common.Validation;

/// <summary>
/// Validates SMS configuration settings.
/// </summary>
public class SmsSettingsValidator : IConfigurationValidator
{
    /// <inheritdoc/>
    public string ConfigurationName => "SmsSettings";

    /// <inheritdoc/>
    public ValidationResult Validate(IConfiguration configuration)
    {
        var result = new ValidationResult { IsValid = true };
        var section = configuration.GetSection("SmsSettings");

        if (!section.Exists())
        {
            result.Warnings.Add("SmsSettings section not found - SMS functionality may not work");
            return result;
        }

        var provider = section["Provider"]?.ToLowerInvariant() ?? "twilio";

        if (provider == "twilio")
        {
            var accountSid = section["TwilioAccountSid"];
            if (string.IsNullOrEmpty(accountSid))
            {
                result.Warnings.Add("SmsSettings:TwilioAccountSid is not configured - SMS sending will fail");
            }
            else if (!accountSid.StartsWith("AC"))
            {
                result.Warnings.Add("SmsSettings:TwilioAccountSid should start with 'AC'");
            }

            var authToken = section["TwilioAuthToken"];
            if (string.IsNullOrEmpty(authToken))
            {
                result.Warnings.Add("SmsSettings:TwilioAuthToken is not configured - SMS sending will fail");
            }

            var phoneNumber = section["TwilioPhoneNumber"];
            if (string.IsNullOrEmpty(phoneNumber))
            {
                result.Warnings.Add("SmsSettings:TwilioPhoneNumber is not configured - SMS sending will fail");
            }
        }

        return result;
    }
}
