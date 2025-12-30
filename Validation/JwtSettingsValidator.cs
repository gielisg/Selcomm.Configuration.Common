using Microsoft.Extensions.Configuration;

namespace Selcomm.Configuration.Common.Validation;

/// <summary>
/// Validates JWT configuration settings.
/// </summary>
public class JwtSettingsValidator : IConfigurationValidator
{
    /// <inheritdoc/>
    public string ConfigurationName => "JwtSettings";

    /// <inheritdoc/>
    public ValidationResult Validate(IConfiguration configuration)
    {
        var result = new ValidationResult { IsValid = true };
        var section = configuration.GetSection("JwtSettings");

        if (!section.Exists())
        {
            result.Errors.Add("JwtSettings section is required for authentication");
            result.IsValid = false;
            return result;
        }

        var keyType = section["KeyType"]?.ToLowerInvariant() ?? "hmac";

        if (keyType == "hmac")
        {
            var secretKey = section["SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                result.Errors.Add("JwtSettings:SecretKey is required for HMAC signing");
                result.IsValid = false;
            }
            else if (secretKey.Length < 32)
            {
                result.Errors.Add("JwtSettings:SecretKey must be at least 32 characters for security");
                result.IsValid = false;
            }
        }
        else if (keyType == "rsa")
        {
            var keyPath = section["RsaPrivateKeyPath"];
            var keyPem = section["RsaPrivateKeyPem"];

            if (string.IsNullOrEmpty(keyPath) && string.IsNullOrEmpty(keyPem))
            {
                result.Errors.Add("JwtSettings:RsaPrivateKeyPath or RsaPrivateKeyPem is required for RSA signing");
                result.IsValid = false;
            }
            else if (!string.IsNullOrEmpty(keyPath) && !File.Exists(keyPath))
            {
                result.Errors.Add($"JwtSettings:RsaPrivateKeyPath file not found: {keyPath}");
                result.IsValid = false;
            }
        }
        else
        {
            result.Errors.Add($"JwtSettings:KeyType '{keyType}' is not valid. Use 'hmac' or 'rsa'");
            result.IsValid = false;
        }

        // Validate expiration settings
        var accessExpStr = section["AccessTokenExpirationMinutes"];
        if (!string.IsNullOrEmpty(accessExpStr) && int.TryParse(accessExpStr, out var accessExp))
        {
            if (accessExp < 1 || accessExp > 1440)
            {
                result.Warnings.Add("JwtSettings:AccessTokenExpirationMinutes should be between 1 and 1440 (24 hours)");
            }
        }

        var refreshExpStr = section["RefreshTokenExpirationDays"];
        if (!string.IsNullOrEmpty(refreshExpStr) && int.TryParse(refreshExpStr, out var refreshExp))
        {
            if (refreshExp < 1 || refreshExp > 365)
            {
                result.Warnings.Add("JwtSettings:RefreshTokenExpirationDays should be between 1 and 365");
            }
        }

        return result;
    }
}
