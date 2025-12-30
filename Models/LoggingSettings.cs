namespace Selcomm.Configuration.Common.Models;

/// <summary>
/// Serilog and logging configuration settings.
/// Supports both standard .NET logging and Serilog configuration.
/// </summary>
public class LoggingSettings
{
    /// <summary>Serilog-specific settings.</summary>
    public SerilogSettings Serilog { get; set; } = new();

    /// <summary>Standard .NET logging settings.</summary>
    public StandardLoggingSettings Logging { get; set; } = new();
}

/// <summary>
/// Serilog configuration settings.
/// </summary>
public class SerilogSettings
{
    /// <summary>Minimum level configuration.</summary>
    public MinimumLevelSettings MinimumLevel { get; set; } = new();

    /// <summary>Write to console sink.</summary>
    public ConsoleSinkSettings? Console { get; set; }

    /// <summary>Write to file sink.</summary>
    public FileSinkSettings? File { get; set; }

    /// <summary>Additional properties to enrich logs.</summary>
    public Dictionary<string, string> Properties { get; set; } = new();

    /// <summary>Sinks to use (e.g., "Console", "File", "Seq").</summary>
    public List<string> Using { get; set; } = new();

    /// <summary>Enrichers to use (e.g., "FromLogContext", "WithMachineName").</summary>
    public List<string> Enrich { get; set; } = new();
}

/// <summary>
/// Minimum level settings for Serilog.
/// </summary>
public class MinimumLevelSettings
{
    /// <summary>Default minimum log level.</summary>
    public string Default { get; set; } = "Information";

    /// <summary>Override log levels for specific namespaces.</summary>
    public Dictionary<string, string> Override { get; set; } = new()
    {
        ["Microsoft.AspNetCore"] = "Warning",
        ["Microsoft.AspNetCore.Hosting"] = "Warning",
        ["Microsoft.AspNetCore.Routing"] = "Warning",
        ["System"] = "Warning"
    };
}

/// <summary>
/// Console sink settings for Serilog.
/// </summary>
public class ConsoleSinkSettings
{
    /// <summary>Whether the console sink is enabled.</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>Output template for console logging.</summary>
    public string? OutputTemplate { get; set; }

    /// <summary>Theme to use (e.g., "Literate", "Code", "Grayscale").</summary>
    public string? Theme { get; set; }

    /// <summary>Minimum level for console output (overrides global).</summary>
    public string? RestrictedToMinimumLevel { get; set; }
}

/// <summary>
/// File sink settings for Serilog.
/// </summary>
public class FileSinkSettings
{
    /// <summary>Whether the file sink is enabled.</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>Path to the log file.</summary>
    public string Path { get; set; } = "Logs/app-.log";

    /// <summary>Rolling interval for log files.</summary>
    public string RollingInterval { get; set; } = "Day";

    /// <summary>Maximum file size before rolling (e.g., "10MB").</summary>
    public long? FileSizeLimitBytes { get; set; } = 10_485_760; // 10MB

    /// <summary>Number of files to retain.</summary>
    public int? RetainedFileCountLimit { get; set; } = 31;

    /// <summary>Output template for file logging.</summary>
    public string? OutputTemplate { get; set; }

    /// <summary>Whether to use buffered writing.</summary>
    public bool Buffered { get; set; } = false;

    /// <summary>Whether to use shared mode for file access.</summary>
    public bool Shared { get; set; } = false;

    /// <summary>Minimum level for file output (overrides global).</summary>
    public string? RestrictedToMinimumLevel { get; set; }

    /// <summary>Whether to roll on file size limit.</summary>
    public bool RollOnFileSizeLimit { get; set; } = true;
}

/// <summary>
/// Standard .NET logging settings.
/// </summary>
public class StandardLoggingSettings
{
    /// <summary>Log level configuration.</summary>
    public Dictionary<string, string> LogLevel { get; set; } = new()
    {
        ["Default"] = "Information",
        ["Microsoft.AspNetCore"] = "Warning"
    };
}
