using CsCheck.Extension.Generators;
using CsCheck.Extension.Generators.Options;

namespace CsCheck.Extension.Builder;

/// <summary>
/// Generator for creating random email addresses.
/// </summary>
public class EmailGenBuilder : IEmailGenBuilder
{
    private EmailGenOptions Options { get; init; } = new EmailGenOptions();

    public IEmailGenBuilder AllowIPv4()
    {
        Options.AllowIPv4();
        return this;
    }

    public IEmailGenBuilder AllowQuotedLocalPart()
    {
        Options.AllowQuotedLocalPart();
        return this;
    }

    public GenEmail Build()
    {
        return new GenEmail(Options);
    }

    public IEmailGenBuilder PolluteEmails()
    {
        Options.PolluteEmails();
        return this;
    }
}
