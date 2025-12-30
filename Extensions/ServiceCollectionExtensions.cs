using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Selcomm.Configuration.Common.Abstractions;
using Selcomm.Configuration.Common.Caching;
using Selcomm.Configuration.Common.Models;
using Selcomm.Configuration.Common.Providers;
using Selcomm.Configuration.Common.Validation;

namespace Selcomm.Configuration.Common.Extensions;

/// <summary>
/// Extension methods for registering Selcomm configuration services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all Selcomm configuration providers to the service collection.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Application configuration</param>
    /// <param name="configure">Optional configuration action</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddSelcommConfiguration(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<SelcommConfigurationOptions>? configure = null)
    {
        var options = new SelcommConfigurationOptions();
        configure?.Invoke(options);

        // Register options
        services.AddSingleton(options);

        // Register configuration caches if caching is enabled
        if (options.EnableCaching)
        {
            // Register specific caches for each configuration type
            services.AddSingleton<IConfigurationCache<EmailSettings>>(_ => new ConfigurationCache<EmailSettings>(options.CacheExpiration));
            services.AddSingleton<IConfigurationCache<SmsSettings>>(_ => new ConfigurationCache<SmsSettings>(options.CacheExpiration));
            services.AddSingleton<IConfigurationCache<JwtSettings>>(_ => new ConfigurationCache<JwtSettings>(options.CacheExpiration));
            services.AddSingleton<IConfigurationCache<PasswordPolicySettings>>(_ => new ConfigurationCache<PasswordPolicySettings>(options.CacheExpiration));
            services.AddSingleton<IConfigurationCache<PasswordResetSettings>>(_ => new ConfigurationCache<PasswordResetSettings>(options.CacheExpiration));
        }

        // Register configuration providers
        services.AddSingleton<IDomainConfigurationProvider<EmailSettings>>(sp =>
            new EmailConfigurationProvider(
                configuration,
                sp.GetRequiredService<ILogger<EmailConfigurationProvider>>(),
                options.EnableCaching ? sp.GetService<IConfigurationCache<EmailSettings>>() : null));

        services.AddSingleton<IDomainConfigurationProvider<SmsSettings>>(sp =>
            new SmsConfigurationProvider(
                configuration,
                sp.GetRequiredService<ILogger<SmsConfigurationProvider>>(),
                options.EnableCaching ? sp.GetService<IConfigurationCache<SmsSettings>>() : null));

        services.AddSingleton<IDomainConfigurationProvider<JwtSettings>>(sp =>
            new JwtConfigurationProvider(
                configuration,
                sp.GetRequiredService<ILogger<JwtConfigurationProvider>>(),
                options.EnableCaching ? sp.GetService<IConfigurationCache<JwtSettings>>() : null));

        services.AddSingleton<IDomainConfigurationProvider<PasswordPolicySettings>>(sp =>
            new PasswordPolicyProvider(
                configuration,
                sp.GetRequiredService<ILogger<PasswordPolicyProvider>>(),
                options.EnableCaching ? sp.GetService<IConfigurationCache<PasswordPolicySettings>>() : null));

        services.AddSingleton<IDomainConfigurationProvider<PasswordResetSettings>>(sp =>
            new PasswordResetConfigurationProvider(
                configuration,
                sp.GetRequiredService<ILogger<PasswordResetConfigurationProvider>>(),
                options.EnableCaching ? sp.GetService<IConfigurationCache<PasswordResetSettings>>() : null));

        // Register database connection provider
        services.AddSingleton<IDatabaseConnectionProvider>(sp =>
            new DatabaseConnectionProvider(
                configuration,
                sp.GetRequiredService<ILogger<DatabaseConnectionProvider>>()));

        // Register security policy provider
        services.AddSingleton<ISecurityPolicyProvider>(sp =>
            new SecurityPolicyProvider(
                sp.GetRequiredService<ILogger<SecurityPolicyProvider>>(),
                options.SecurityPolicyBasePath));

        // Register logging configuration provider
        services.AddSingleton<ILoggingConfigurationProvider>(sp =>
            new LoggingConfigurationProvider(configuration));

        // Register IOptions<T> bindings for simple scenarios
        OptionsConfigurationServiceCollectionExtensions.Configure<OtpSettings>(services, configuration.GetSection("OtpSettings"));
        OptionsConfigurationServiceCollectionExtensions.Configure<MockServiceSettings>(services, configuration.GetSection("MockServices"));
        OptionsConfigurationServiceCollectionExtensions.Configure<EmailConfirmationSettings>(services, configuration.GetSection("EmailConfirmationSettings"));
        OptionsConfigurationServiceCollectionExtensions.Configure<MobileConfirmationSettings>(services, configuration.GetSection("MobileConfirmationSettings"));

        // Register configuration validators
        if (options.ValidateOnStartup)
        {
            services.AddTransient<IConfigurationValidator, EmailSettingsValidator>();
            services.AddTransient<IConfigurationValidator, SmsSettingsValidator>();
            services.AddTransient<IConfigurationValidator, JwtSettingsValidator>();
            services.AddTransient<IConfigurationValidator, DatabaseSettingsValidator>();
            services.AddHostedService<ConfigurationValidationHostedService>();
        }

        return services;
    }

    /// <summary>
    /// Adds configuration validation services without the full configuration setup.
    /// </summary>
    public static IServiceCollection AddSelcommConfigurationValidation(
        this IServiceCollection services)
    {
        services.AddTransient<IConfigurationValidator, EmailSettingsValidator>();
        services.AddTransient<IConfigurationValidator, SmsSettingsValidator>();
        services.AddTransient<IConfigurationValidator, JwtSettingsValidator>();
        services.AddTransient<IConfigurationValidator, DatabaseSettingsValidator>();

        return services;
    }
}

/// <summary>
/// Hosted service that validates configuration on startup.
/// </summary>
public class ConfigurationValidationHostedService : IHostedService
{
    private readonly IEnumerable<IConfigurationValidator> _validators;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ConfigurationValidationHostedService> _logger;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly SelcommConfigurationOptions _options;

    public ConfigurationValidationHostedService(
        IEnumerable<IConfigurationValidator> validators,
        IConfiguration configuration,
        ILogger<ConfigurationValidationHostedService> logger,
        IHostApplicationLifetime lifetime,
        SelcommConfigurationOptions options)
    {
        _validators = validators;
        _configuration = configuration;
        _logger = logger;
        _lifetime = lifetime;
        _options = options;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var allValid = true;

        foreach (var validator in _validators)
        {
            var result = validator.Validate(_configuration);

            foreach (var warning in result.Warnings)
            {
                _logger.LogWarning("[{Config}] {Warning}", validator.ConfigurationName, warning);
            }

            foreach (var error in result.Errors)
            {
                if (_options.FailOnValidationErrors)
                {
                    _logger.LogError("[{Config}] {Error}", validator.ConfigurationName, error);
                }
                else
                {
                    _logger.LogWarning("[{Config}] Validation error (non-fatal): {Error}",
                        validator.ConfigurationName, error);
                }
            }

            if (!result.IsValid)
            {
                allValid = false;
            }
        }

        if (!allValid && _options.FailOnValidationErrors)
        {
            _logger.LogCritical("Configuration validation failed. Application startup aborted.");
            _lifetime.StopApplication();
        }
        else if (allValid)
        {
            _logger.LogInformation("Configuration validation passed");
        }
        else
        {
            _logger.LogWarning("Configuration validation completed with warnings/errors (non-fatal mode)");
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
