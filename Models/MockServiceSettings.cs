namespace Selcomm.Configuration.Common.Models;

/// <summary>
/// Mock services configuration for testing.
/// </summary>
public class MockServiceSettings
{
    /// <summary>Enable mock email/SMS services.</summary>
    public bool Enabled { get; set; } = false;

    /// <summary>Log mock messages to console.</summary>
    public bool LogToConsole { get; set; } = true;

    /// <summary>Log mock messages to file.</summary>
    public bool LogToFile { get; set; } = true;

    /// <summary>Path for mock email log file.</summary>
    public string MockEmailLogPath { get; set; } = "Logs/mock-emails.log";

    /// <summary>Path for mock SMS log file.</summary>
    public string MockSmsLogPath { get; set; } = "Logs/mock-sms.log";
}
