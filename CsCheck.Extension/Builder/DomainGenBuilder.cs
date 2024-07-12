using CsCheck.Extension.Generators;
using CsCheck.Extension.Generators.Options;

namespace CsCheck.Extension.Builder;

/// <summary>
/// Generator for creating random domain names.
/// </summary>
public class DomainGenBuilder : IDomainGenBuilder
{
    private DomainGenOptions Options { get; init; } = new DomainGenOptions();

    public GenDomain Build()
    {
        return new GenDomain(Options);
    }

    public IDomainGenBuilder DenyHostNames()
    {
        Options.DenyHostNames();
        return this;
    }
}
