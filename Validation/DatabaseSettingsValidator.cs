using Microsoft.Extensions.Configuration;

namespace Selcomm.Configuration.Common.Validation;

/// <summary>
/// Validates database connection string configuration.
/// </summary>
public class DatabaseSettingsValidator : IConfigurationValidator
{
    /// <inheritdoc/>
    public string ConfigurationName => "DomainConnectionStrings";

    /// <inheritdoc/>
    public ValidationResult Validate(IConfiguration configuration)
    {
        var result = new ValidationResult { IsValid = true };
        var section = configuration.GetSection("DomainConnectionStrings");

        if (!section.Exists())
        {
            result.Errors.Add("DomainConnectionStrings section is required for database connectivity");
            result.IsValid = false;
            return result;
        }

        var domains = section.GetChildren().ToList();
        if (domains.Count == 0)
        {
            result.Errors.Add("DomainConnectionStrings has no domains configured");
            result.IsValid = false;
            return result;
        }

        foreach (var domain in domains)
        {
            var connectionString = domain.Value;
            if (string.IsNullOrEmpty(connectionString))
            {
                result.Errors.Add($"DomainConnectionStrings:{domain.Key} has no connection string");
                result.IsValid = false;
            }
        }

        return result;
    }
}
