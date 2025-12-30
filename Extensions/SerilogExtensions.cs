using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Selcomm.Configuration.Common.Abstractions;
using Selcomm.Configuration.Common.Models;

namespace Selcomm.Configuration.Common.Extensions;

/// <summary>
/// Extension methods for configuring Serilog using Selcomm configuration.
/// </summary>
public static class SerilogExtensions
{
    /// <summary>
    /// Configures Serilog using the standard configuration from IConfiguration.
    /// This is the recommended approach for most applications.
    /// </summary>
    /// <param name="builder">The host builder.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The host builder for chaining.</returns>
    public static IHostBuilder UseSelcommSerilog(this IHostBuilder builder, IConfiguration configuration)
    {
        return builder.UseSerilog((context, services, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(configuration);
        });
    }

    /// <summary>
    /// Configures Serilog using the LoggingConfigurationProvider for more control.
    /// Use this when you need programmatic access to logging settings.
    /// </summary>
    /// <param name="builder">The host builder.</param>
    /// <param name="loggingProvider">The logging configuration provider.</param>
    /// <param name="additionalConfig">Optional additional configuration action.</param>
    /// <returns>The host builder for chaining.</returns>
    public static IHostBuilder UseSelcommSerilog(
        this IHostBuilder builder,
        ILoggingConfigurationProvider loggingProvider,
        Action<LoggerConfiguration>? additionalConfig = null)
    {
        return builder.UseSerilog((context, services, loggerConfiguration) =>
        {
            var settings = loggingProvider.GetSerilogSettings();
            ConfigureFromSettings(loggerConfiguration, settings);
            additionalConfig?.Invoke(loggerConfiguration);
        });
    }

    /// <summary>
    /// Creates a bootstrap logger for use during application startup.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>A configured Serilog logger.</returns>
    public static ILogger CreateBootstrapLogger(IConfiguration configuration)
    {
        return new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateBootstrapLogger();
    }

    /// <summary>
    /// Creates a bootstrap logger with specified minimum level.
    /// </summary>
    /// <param name="minimumLevel">The minimum log level.</param>
    /// <param name="writeToConsole">Whether to write to console.</param>
    /// <returns>A configured Serilog logger.</returns>
    public static ILogger CreateBootstrapLogger(
        LogEventLevel minimumLevel = LogEventLevel.Information,
        bool writeToConsole = true)
    {
        var config = new LoggerConfiguration()
            .MinimumLevel.Is(minimumLevel);

        if (writeToConsole)
        {
            config.WriteTo.Console();
        }

        return config.CreateBootstrapLogger();
    }

    /// <summary>
    /// Configures LoggerConfiguration from SerilogSettings.
    /// </summary>
    private static void ConfigureFromSettings(LoggerConfiguration loggerConfig, SerilogSettings settings)
    {
        // Set minimum level
        var defaultLevel = ParseLogLevel(settings.MinimumLevel.Default);
        loggerConfig.MinimumLevel.Is(defaultLevel);

        // Set overrides
        foreach (var (source, level) in settings.MinimumLevel.Override)
        {
            var overrideLevel = ParseLogLevel(level);
            loggerConfig.MinimumLevel.Override(source, overrideLevel);
        }

        // Add enrichers
        loggerConfig.Enrich.FromLogContext();

        if (settings.Enrich.Contains("WithMachineName", StringComparer.OrdinalIgnoreCase))
        {
            loggerConfig.Enrich.WithProperty("MachineName", Environment.MachineName);
        }

        if (settings.Enrich.Contains("WithEnvironmentName", StringComparer.OrdinalIgnoreCase))
        {
            loggerConfig.Enrich.WithProperty("Environment",
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production");
        }

        // Add custom properties
        foreach (var (key, value) in settings.Properties)
        {
            loggerConfig.Enrich.WithProperty(key, value);
        }

        // Configure console sink
        if (settings.Console?.Enabled == true)
        {
            var consoleLevel = settings.Console.RestrictedToMinimumLevel != null
                ? ParseLogLevel(settings.Console.RestrictedToMinimumLevel)
                : defaultLevel;

            if (!string.IsNullOrEmpty(settings.Console.OutputTemplate))
            {
                loggerConfig.WriteTo.Console(
                    restrictedToMinimumLevel: consoleLevel,
                    outputTemplate: settings.Console.OutputTemplate);
            }
            else
            {
                loggerConfig.WriteTo.Console(restrictedToMinimumLevel: consoleLevel);
            }
        }

        // Configure file sink
        if (settings.File?.Enabled == true)
        {
            var fileLevel = settings.File.RestrictedToMinimumLevel != null
                ? ParseLogLevel(settings.File.RestrictedToMinimumLevel)
                : defaultLevel;

            var rollingInterval = ParseRollingInterval(settings.File.RollingInterval);

            loggerConfig.WriteTo.File(
                path: settings.File.Path,
                restrictedToMinimumLevel: fileLevel,
                rollingInterval: rollingInterval,
                fileSizeLimitBytes: settings.File.FileSizeLimitBytes,
                retainedFileCountLimit: settings.File.RetainedFileCountLimit,
                buffered: settings.File.Buffered,
                shared: settings.File.Shared,
                rollOnFileSizeLimit: settings.File.RollOnFileSizeLimit,
                outputTemplate: settings.File.OutputTemplate ??
                    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}");
        }
    }

    private static LogEventLevel ParseLogLevel(string level)
    {
        return level?.ToLowerInvariant() switch
        {
            "verbose" or "trace" => LogEventLevel.Verbose,
            "debug" => LogEventLevel.Debug,
            "information" or "info" => LogEventLevel.Information,
            "warning" or "warn" => LogEventLevel.Warning,
            "error" => LogEventLevel.Error,
            "fatal" or "critical" => LogEventLevel.Fatal,
            _ => LogEventLevel.Information
        };
    }

    private static RollingInterval ParseRollingInterval(string interval)
    {
        return interval?.ToLowerInvariant() switch
        {
            "infinite" => RollingInterval.Infinite,
            "year" => RollingInterval.Year,
            "month" => RollingInterval.Month,
            "day" => RollingInterval.Day,
            "hour" => RollingInterval.Hour,
            "minute" => RollingInterval.Minute,
            _ => RollingInterval.Day
        };
    }
}
