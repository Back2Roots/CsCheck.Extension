using CsCheck.Extension.Builder.Common;
using CsCheck.Extension.Generators;

namespace CsCheck.Extension.Builder;

public interface IEmailGenBuilder : IGenBuilder<GenEmail>
{
    public IEmailGenBuilder AllowQuotedLocalPart();
    public IEmailGenBuilder AllowIPv4();
    public IEmailGenBuilder PolluteEmails();
}
