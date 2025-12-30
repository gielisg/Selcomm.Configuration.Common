namespace Selcomm.Configuration.Common.Models;

/// <summary>
/// Database configuration settings.
/// </summary>
public class DatabaseSettings
{
    /// <summary>Database provider type (e.g., "ODBC", "SqlServer").</summary>
    public string Provider { get; set; } = "ODBC";

    /// <summary>Connection string for the database.</summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>Command timeout in seconds.</summary>
    public int CommandTimeout { get; set; } = 30;

    /// <summary>Enable connection pooling.</summary>
    public bool EnablePooling { get; set; } = true;

    /// <summary>Minimum pool size.</summary>
    public int MinPoolSize { get; set; } = 0;

    /// <summary>Maximum pool size.</summary>
    public int MaxPoolSize { get; set; } = 100;
}
