using Selcomm.Configuration.Common.Models;

namespace Selcomm.Configuration.Common.Abstractions;

/// <summary>
/// Provides security policy management with file-based storage.
/// </summary>
public interface ISecurityPolicyProvider
{
    /// <summary>
    /// Gets the security policy for the specified domain.
    /// </summary>
    /// <param name="domain">Domain identifier</param>
    /// <returns>Security policy for the domain</returns>
    SecurityPolicy GetPolicy(string domain);

    /// <summary>
    /// Updates the security policy for a domain.
    /// </summary>
    /// <param name="domain">Domain identifier</param>
    /// <param name="policy">Updated security policy</param>
    /// <returns>True if update was successful</returns>
    Task<bool> UpdatePolicyAsync(string domain, SecurityPolicy policy);

    /// <summary>
    /// Invalidates the cached policy for a specific domain.
    /// </summary>
    /// <param name="domain">Domain identifier</param>
    void InvalidateCache(string domain);

    /// <summary>
    /// Invalidates all cached policies.
    /// </summary>
    void InvalidateAllCaches();

    /// <summary>
    /// Gets all domains that have a security policy configured.
    /// </summary>
    /// <returns>List of domain identifiers</returns>
    IReadOnlyList<string> GetConfiguredDomains();
}
