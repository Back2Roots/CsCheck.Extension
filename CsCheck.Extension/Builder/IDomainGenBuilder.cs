using CsCheck.Extension.Builder.Common;
using CsCheck.Extension.Generators;

namespace CsCheck.Extension.Builder;

public interface IDomainGenBuilder : IGenBuilder<GenDomain>
{
    public IDomainGenBuilder DenyHostNames();
}
