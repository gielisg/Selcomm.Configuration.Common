using Selcomm.Configuration.Common.Models;

namespace Selcomm.Configuration.Common.Abstractions;

/// <summary>
/// Provider for logging configuration settings.
/// Unlike other domain-based providers, logging is typically application-wide
/// but can be customized per environment.
/// </summary>
public interface ILoggingConfigurationProvider
{
    /// <summary>
    /// Gets the current logging settings.
    /// </summary>
    LoggingSettings GetSettings();

    /// <summary>
    /// Gets the Serilog settings.
    /// </summary>
    SerilogSettings GetSerilogSettings();

    /// <summary>
    /// Gets the standard .NET logging settings.
    /// </summary>
    StandardLoggingSettings GetStandardLoggingSettings();

    /// <summary>
    /// Gets the minimum log level for a specific category/namespace.
    /// </summary>
    /// <param name="category">The logging category (namespace).</param>
    /// <returns>The log level as a string (e.g., "Information", "Warning").</returns>
    string GetLogLevelForCategory(string category);

    /// <summary>
    /// Checks if a specific log level is enabled for a category.
    /// </summary>
    /// <param name="category">The logging category.</param>
    /// <param name="logLevel">The log level to check.</param>
    /// <returns>True if the log level is enabled.</returns>
    bool IsEnabled(string category, string logLevel);

    /// <summary>
    /// Refreshes the configuration from the source.
    /// </summary>
    void Refresh();
}
