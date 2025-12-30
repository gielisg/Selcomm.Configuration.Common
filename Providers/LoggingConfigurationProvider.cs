using Microsoft.Extensions.Configuration;
using Selcomm.Configuration.Common.Abstractions;
using Selcomm.Configuration.Common.Models;

namespace Selcomm.Configuration.Common.Providers;

/// <summary>
/// Provider for logging configuration from IConfiguration.
/// </summary>
public class LoggingConfigurationProvider : ILoggingConfigurationProvider
{
    private readonly IConfiguration _configuration;
    private LoggingSettings? _cachedSettings;
    private readonly object _lock = new();

    // Log level hierarchy for comparison
    private static readonly Dictionary<string, int> LogLevelHierarchy = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Verbose"] = 0,
        ["Debug"] = 1,
        ["Information"] = 2,
        ["Warning"] = 3,
        ["Error"] = 4,
        ["Fatal"] = 5,
        // .NET aliases
        ["Trace"] = 0,
        ["None"] = 6
    };

    public LoggingConfigurationProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public LoggingSettings GetSettings()
    {
        if (_cachedSettings != null)
            return _cachedSettings;

        lock (_lock)
        {
            if (_cachedSettings != null)
                return _cachedSettings;

            _cachedSettings = BuildSettings();
            return _cachedSettings;
        }
    }

    public SerilogSettings GetSerilogSettings()
    {
        return GetSettings().Serilog;
    }

    public StandardLoggingSettings GetStandardLoggingSettings()
    {
        return GetSettings().Logging;
    }

    public string GetLogLevelForCategory(string category)
    {
        var serilog = GetSerilogSettings();

        // Check for exact match first
        if (serilog.MinimumLevel.Override.TryGetValue(category, out var level))
            return level;

        // Check for prefix matches (e.g., "Microsoft.AspNetCore" matches "Microsoft.AspNetCore.Hosting")
        var matchingKey = serilog.MinimumLevel.Override.Keys
            .Where(k => category.StartsWith(k, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(k => k.Length)
            .FirstOrDefault();

        if (matchingKey != null)
            return serilog.MinimumLevel.Override[matchingKey];

        // Fall back to default
        return serilog.MinimumLevel.Default;
    }

    public bool IsEnabled(string category, string logLevel)
    {
        var categoryLevel = GetLogLevelForCategory(category);

        if (!LogLevelHierarchy.TryGetValue(categoryLevel, out var categoryLevelValue))
            categoryLevelValue = 2; // Default to Information

        if (!LogLevelHierarchy.TryGetValue(logLevel, out var requestedLevelValue))
            requestedLevelValue = 2;

        return requestedLevelValue >= categoryLevelValue;
    }

    public void Refresh()
    {
        lock (_lock)
        {
            _cachedSettings = null;
        }
    }

    private LoggingSettings BuildSettings()
    {
        var settings = new LoggingSettings();

        // Load Serilog settings
        var serilogSection = _configuration.GetSection("Serilog");
        if (serilogSection.Exists())
        {
            // MinimumLevel
            var minLevelSection = serilogSection.GetSection("MinimumLevel");
            if (minLevelSection.Exists())
            {
                settings.Serilog.MinimumLevel.Default = minLevelSection["Default"] ?? "Information";

                var overrideSection = minLevelSection.GetSection("Override");
                if (overrideSection.Exists())
                {
                    settings.Serilog.MinimumLevel.Override = overrideSection
                        .GetChildren()
                        .ToDictionary(x => x.Key, x => x.Value ?? "Information");
                }
            }

            // Using
            var usingSection = serilogSection.GetSection("Using");
            if (usingSection.Exists())
            {
                settings.Serilog.Using = usingSection.GetChildren()
                    .Select(x => x.Value ?? "")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();
            }

            // Enrich
            var enrichSection = serilogSection.GetSection("Enrich");
            if (enrichSection.Exists())
            {
                settings.Serilog.Enrich = enrichSection.GetChildren()
                    .Select(x => x.Value ?? "")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();
            }

            // Properties
            var propertiesSection = serilogSection.GetSection("Properties");
            if (propertiesSection.Exists())
            {
                settings.Serilog.Properties = propertiesSection
                    .GetChildren()
                    .ToDictionary(x => x.Key, x => x.Value ?? "");
            }

            // WriteTo sinks (simplified - for Console and File)
            var writeToSection = serilogSection.GetSection("WriteTo");
            if (writeToSection.Exists())
            {
                foreach (var sink in writeToSection.GetChildren())
                {
                    var sinkName = sink["Name"];
                    var argsSection = sink.GetSection("Args");

                    if (sinkName?.Equals("Console", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        settings.Serilog.Console = new ConsoleSinkSettings
                        {
                            Enabled = true,
                            OutputTemplate = argsSection["outputTemplate"],
                            Theme = argsSection["theme"],
                            RestrictedToMinimumLevel = argsSection["restrictedToMinimumLevel"]
                        };
                    }
                    else if (sinkName?.Equals("File", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        settings.Serilog.File = new FileSinkSettings
                        {
                            Enabled = true,
                            Path = argsSection["path"] ?? "Logs/app-.log",
                            RollingInterval = argsSection["rollingInterval"] ?? "Day",
                            OutputTemplate = argsSection["outputTemplate"],
                            RestrictedToMinimumLevel = argsSection["restrictedToMinimumLevel"],
                            Shared = bool.TryParse(argsSection["shared"], out var shared) && shared,
                            Buffered = bool.TryParse(argsSection["buffered"], out var buffered) && buffered
                        };

                        if (long.TryParse(argsSection["fileSizeLimitBytes"], out var sizeLimit))
                            settings.Serilog.File.FileSizeLimitBytes = sizeLimit;

                        if (int.TryParse(argsSection["retainedFileCountLimit"], out var retainedCount))
                            settings.Serilog.File.RetainedFileCountLimit = retainedCount;
                    }
                }
            }
        }

        // Load standard Logging settings
        var loggingSection = _configuration.GetSection("Logging");
        if (loggingSection.Exists())
        {
            var logLevelSection = loggingSection.GetSection("LogLevel");
            if (logLevelSection.Exists())
            {
                settings.Logging.LogLevel = logLevelSection
                    .GetChildren()
                    .ToDictionary(x => x.Key, x => x.Value ?? "Information");
            }
        }

        return settings;
    }
}
