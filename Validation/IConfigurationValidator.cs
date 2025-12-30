using Microsoft.Extensions.Configuration;

namespace Selcomm.Configuration.Common.Validation;

/// <summary>
/// Interface for configuration validators.
/// </summary>
public interface IConfigurationValidator
{
    /// <summary>
    /// Name of the configuration being validated.
    /// </summary>
    string ConfigurationName { get; }

    /// <summary>
    /// Validates the configuration.
    /// </summary>
    /// <param name="configuration">Application configuration</param>
    /// <returns>Validation result</returns>
    ValidationResult Validate(IConfiguration configuration);
}

/// <summary>
/// Result of configuration validation.
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// Whether the configuration is valid.
    /// </summary>
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// Validation errors (fatal issues).
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// Validation warnings (non-fatal issues).
    /// </summary>
    public List<string> Warnings { get; set; } = new();

    /// <summary>
    /// Creates a successful validation result.
    /// </summary>
    public static ValidationResult Success() => new() { IsValid = true };

    /// <summary>
    /// Creates a failed validation result with an error.
    /// </summary>
    public static ValidationResult Failure(string error) => new()
    {
        IsValid = false,
        Errors = new List<string> { error }
    };
}
