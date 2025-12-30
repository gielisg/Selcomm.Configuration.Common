namespace Selcomm.Configuration.Common.Models;

/// <summary>
/// JWT authentication configuration settings.
/// </summary>
public class JwtSettings
{
    /// <summary>Key type: "hmac" or "rsa".</summary>
    public string KeyType { get; set; } = "hmac";

    /// <summary>Secret key for HMAC signing (minimum 32 characters).</summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>File path to RSA private key (for RSA signing).</summary>
    public string? RsaPrivateKeyPath { get; set; }

    /// <summary>RSA private key in PEM format (alternative to file path).</summary>
    public string? RsaPrivateKeyPem { get; set; }

    /// <summary>JWT issuer claim.</summary>
    public string Issuer { get; set; } = "AuthenticationApi";

    /// <summary>JWT audience claim.</summary>
    public string Audience { get; set; } = "AuthenticationApiClient";

    /// <summary>Access token expiration in minutes.</summary>
    public int AccessTokenExpirationMinutes { get; set; } = 15;

    /// <summary>Refresh token expiration in days.</summary>
    public int RefreshTokenExpirationDays { get; set; } = 7;

    /// <summary>Anonymous token expiration in minutes.</summary>
    public int AnonymousTokenExpirationMinutes { get; set; } = 10;

    /// <summary>Whether to rotate refresh tokens on use.</summary>
    public bool RotateRefreshToken { get; set; } = false;
}
