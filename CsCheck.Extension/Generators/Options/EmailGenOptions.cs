namespace CsCheck.Extension.Generators.Options;

/// <summary>
/// Options that should affect email generator behaviour.
/// </summary>
public class EmailGenOptions
{
    public bool IPv4Enabled { get; private set; } = false;
    public bool QuotedLocalPartEnabled { get; private set; } = false;
    public bool PollutedEmailsEnabled { get; private set; } = false;

    /// <summary>
    /// Allows to generated email addresses with quoted local part.
    /// </summary>
    public EmailGenOptions AllowQuotedLocalPart()
    {
        QuotedLocalPartEnabled = true;
        return this;
    }

    /// <summary>
    /// Allows the generation of email addresses that can have an ipv4 address as domain.
    /// </summary>
    public EmailGenOptions AllowIPv4()
    {
        IPv4Enabled = true;
        return this;
    }

    /// <summary>
    /// Changes the generation of email address by trying to pollute it after generation process
    /// in different ways to make them invalid.
    /// </summary>
    public EmailGenOptions PolluteEmails()
    {
        PollutedEmailsEnabled = true;
        return this;
    }
}

