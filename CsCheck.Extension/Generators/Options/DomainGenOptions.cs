namespace CsCheck.Extension.Generators.Options;

/// <summary>
/// Options that should affect domain generator behaviour.
/// </summary>
public class DomainGenOptions
{
    public bool HostNamesEnabled { get; private set; } = true;

    /// <summary>
    /// Creates domain names with at least two labels.
    /// </summary>
    /// <returns></returns>
    public DomainGenOptions DenyHostNames()
    {
        HostNamesEnabled = false;
        return this;
    }
}
