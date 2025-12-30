namespace Selcomm.Configuration.Common.Models;

/// <summary>
/// Email/SMTP configuration settings with branding support.
/// </summary>
public class EmailSettings
{
    /// <summary>SMTP server hostname.</summary>
    public string SmtpServer { get; set; } = string.Empty;

    /// <summary>SMTP server port (default: 587 for TLS).</summary>
    public int SmtpPort { get; set; } = 587;

    /// <summary>Sender email address.</summary>
    public string SenderEmail { get; set; } = string.Empty;

    /// <summary>Sender display name.</summary>
    public string SenderName { get; set; } = string.Empty;

    /// <summary>SMTP authentication username.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>SMTP authentication password.</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>Enable SSL/TLS for SMTP connection.</summary>
    public bool EnableSsl { get; set; } = true;

    // Branding (typically domain-specific)

    /// <summary>Company name for email branding.</summary>
    public string? CompanyName { get; set; }

    /// <summary>Company logo URL for email templates.</summary>
    public string? LogoUrl { get; set; }

    /// <summary>Support email address displayed in emails.</summary>
    public string? SupportEmail { get; set; }

    /// <summary>Company website URL.</summary>
    public string? WebsiteUrl { get; set; }

    /// <summary>Domain-specific template folder name.</summary>
    public string? TemplateFolder { get; set; }

    /// <summary>Ethereal Web URL for test email viewing (development only).</summary>
    public string? EtherealWebUrl { get; set; }
}
