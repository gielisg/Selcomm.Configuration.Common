using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Selcomm.Configuration.Common.Abstractions;
using Selcomm.Configuration.Common.Models;

namespace Selcomm.Configuration.Common.Providers;

/// <summary>
/// Security policy provider that loads from JSON files with caching.
/// </summary>
public class SecurityPolicyProvider : ISecurityPolicyProvider
{
    private readonly ILogger<SecurityPolicyProvider> _logger;
    private readonly ConcurrentDictionary<string, SecurityPolicy> _cache = new();
    private readonly string _configurationBasePath;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Creates a new security policy provider.
    /// </summary>
    public SecurityPolicyProvider(
        ILogger<SecurityPolicyProvider> logger,
        string configurationBasePath = "Configuration")
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configurationBasePath = configurationBasePath;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    /// <inheritdoc/>
    public SecurityPolicy GetPolicy(string domain)
    {
        if (string.IsNullOrEmpty(domain))
        {
            _logger.LogWarning("Attempted to get security policy with null or empty domain");
            return GetDefaultPolicy("unknown");
        }

        if (_cache.TryGetValue(domain, out var cached))
        {
            return cached;
        }

        var policy = LoadPolicyFromFile(domain);
        _cache[domain] = policy;
        return policy;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdatePolicyAsync(string domain, SecurityPolicy policy)
    {
        if (string.IsNullOrEmpty(domain))
        {
            _logger.LogError("Cannot update security policy: domain is null or empty");
            return false;
        }

        try
        {
            var domainDir = Path.Combine(_configurationBasePath, domain);
            Directory.CreateDirectory(domainDir);

            var path = Path.Combine(domainDir, "security-policy.json");
            var json = JsonSerializer.Serialize(policy, _jsonOptions);

            await File.WriteAllTextAsync(path, json);
            InvalidateCache(domain);

            _logger.LogInformation("Updated security policy for {Domain} at {Path}", domain, path);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update security policy for {Domain}", domain);
            return false;
        }
    }

    /// <inheritdoc/>
    public void InvalidateCache(string domain)
    {
        _cache.TryRemove(domain, out _);
        _logger.LogDebug("Invalidated security policy cache for {Domain}", domain);
    }

    /// <inheritdoc/>
    public void InvalidateAllCaches()
    {
        _cache.Clear();
        _logger.LogInformation("Invalidated all security policy caches");
    }

    /// <inheritdoc/>
    public IReadOnlyList<string> GetConfiguredDomains()
    {
        var domains = new List<string>();

        if (!Directory.Exists(_configurationBasePath))
            return domains;

        foreach (var dir in Directory.GetDirectories(_configurationBasePath))
        {
            var policyPath = Path.Combine(dir, "security-policy.json");
            if (File.Exists(policyPath))
            {
                domains.Add(Path.GetFileName(dir));
            }
        }

        return domains;
    }

    private SecurityPolicy LoadPolicyFromFile(string domain)
    {
        // Try domain-specific first
        var domainPath = Path.Combine(_configurationBasePath, domain, "security-policy.json");
        if (File.Exists(domainPath))
        {
            return LoadFromJson(domainPath, domain);
        }

        // Fall back to default
        var defaultPath = Path.Combine(_configurationBasePath, "default", "security-policy.json");
        if (File.Exists(defaultPath))
        {
            _logger.LogWarning("Using default security policy for domain {Domain}", domain);
            var defaultPolicy = LoadFromJson(defaultPath, domain);
            defaultPolicy.Domain = domain;
            return defaultPolicy;
        }

        _logger.LogWarning("No security policy file found, using hardcoded defaults for {Domain}", domain);
        return GetDefaultPolicy(domain);
    }

    private SecurityPolicy LoadFromJson(string path, string domain)
    {
        try
        {
            var json = File.ReadAllText(path);
            var policy = JsonSerializer.Deserialize<SecurityPolicy>(json, _jsonOptions);

            if (policy == null)
            {
                _logger.LogWarning("Failed to deserialize security policy from {Path}, using defaults", path);
                return GetDefaultPolicy(domain);
            }

            policy.Domain = domain;
            _logger.LogInformation("Loaded security policy for {Domain} from {Path}", domain, path);
            return policy;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading security policy from {Path}", path);
            return GetDefaultPolicy(domain);
        }
    }

    private static SecurityPolicy GetDefaultPolicy(string domain) => new()
    {
        Domain = domain,
        Description = "Default security policy",
        LoginSecurity = new LoginSecuritySettings(),
        PasswordPolicy = new SecurityPolicyPasswordSettings(),
        EmailConfirmation = new SecurityPolicyEmailConfirmationSettings(),
        MobileConfirmation = new SecurityPolicyMobileConfirmationSettings(),
        SessionManagement = new SessionManagementSettings(),
        MfaPolicy = new MfaPolicySettings()
    };
}
