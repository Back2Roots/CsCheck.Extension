using CsCheck.Extension.Generators.Options;

namespace CsCheck.Extension.Generators;

/// <summary>
/// Class <c>GenDomain</c> generates random domain names.
/// </summary>
public sealed class GenDomain : Gen<string>
{
    private static readonly Gen<string> genLabel = Gen.String[
        Gen.Char[
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "abcdefghijklmnopqrstuvwxyz0123456789-"
        ], 1, 63];

    private static readonly Gen<bool> genAnotherLabel = Gen.Bool;

    private DomainGenOptions Options = new DomainGenOptions();

    /// <summary>
    /// Creates a domain generator with default behaviour.
    /// </summary>
    public GenDomain()
    {
    }

    /// <summary>
    /// Creates a domain generator with an customized generating behaviour.
    /// </summary>
    /// <param name="options">Alternative behaviour options</param>
    public GenDomain(DomainGenOptions options)
    {
        Options = options;
    }

    public override string Generate(PCG pcg, Size? min, out Size size)
    {
        var domainName = generateLabel(pcg, min, out size);
        // always generate another label if generating host names option is not activated
        var anotherLabel = Options.HostNamesEnabled == false ? true : genAnotherLabel.Generate(pcg, min, out size);

        while (anotherLabel)
        {
            var label = generateLabel(pcg, min, out size);
            bool maxLengthExceeded = domainName.Length + label.Length + 1 > 255;
            if (maxLengthExceeded)
            {
                break;
            }

            domainName = string.Join(".", label, domainName);
            anotherLabel = genAnotherLabel.Generate(pcg, min, out size);
        }
        return domainName;
    }

    private string generateLabel(PCG pcg, Size? min, out Size size)
    {
        string domainName = genLabel.Generate(pcg, min, out size);
        bool isNotValid = domainName.StartsWith('-') || domainName.EndsWith('-') || domainName.Contains("--");
        return isNotValid
            ? generateLabel(pcg, min, out size)
            : domainName;
    }
}
