using Microsoft.Extensions.Configuration;

namespace Selcomm.Configuration.Common.Validation;

/// <summary>
/// Validates email configuration settings.
/// </summary>
public class EmailSettingsValidator : IConfigurationValidator
{
    /// <inheritdoc/>
    public string ConfigurationName => "EmailSettings";

    /// <inheritdoc/>
    public ValidationResult Validate(IConfiguration configuration)
    {
        var result = new ValidationResult { IsValid = true };
        var section = configuration.GetSection("EmailSettings");

        if (!section.Exists())
        {
            result.Warnings.Add("EmailSettings section not found - email functionality may not work");
            return result;
        }

        var smtpServer = section["SmtpServer"];
        if (string.IsNullOrEmpty(smtpServer))
        {
            result.Errors.Add("EmailSettings:SmtpServer is required");
            result.IsValid = false;
        }

        var senderEmail = section["SenderEmail"];
        if (string.IsNullOrEmpty(senderEmail))
        {
            result.Errors.Add("EmailSettings:SenderEmail is required");
            result.IsValid = false;
        }
        else if (!IsValidEmail(senderEmail))
        {
            result.Errors.Add($"EmailSettings:SenderEmail '{senderEmail}' is not a valid email address");
            result.IsValid = false;
        }

        var portStr = section["SmtpPort"];
        if (!string.IsNullOrEmpty(portStr) && !int.TryParse(portStr, out var port))
        {
            result.Errors.Add($"EmailSettings:SmtpPort '{portStr}' is not a valid port number");
            result.IsValid = false;
        }

        return result;
    }

    private static bool IsValidEmail(string email) =>
        !string.IsNullOrEmpty(email) && email.Contains('@') && email.Contains('.');
}
