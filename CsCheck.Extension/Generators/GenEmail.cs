using CsCheck.Extension.Generators.Options;
using System.Net;

namespace CsCheck.Extension.Generators;

/// <summary>
/// Class <c>GenEmail</c> generates random email adresses.
/// </summary>
public sealed class GenEmail : Gen<string>
{
    private static readonly Gen<string> genDomain = new GenDomain();
    private static readonly Gen<IPAddress> genIpv4 = new GenIPv4Address();
    private static readonly Gen<bool> genBool = Gen.Bool;
    private static readonly Gen<string> genLocalQuoted = Gen.String[1, 62];
    private static readonly Gen<string> genLocal = Gen.String[
        Gen.Char["ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz" +
            "0123456789!#$%&'+-/=^_`{|}~."], 1, 64];

    private EmailGenOptions Options = new EmailGenOptions();

    /// <summary>
    /// Creates an email generator with default behaviour.
    /// </summary>
    public GenEmail()
    {
    }

    /// <summary>
    /// Creates an email generator with an customized generating behaviour.
    /// </summary>
    /// <param name="options">Alternative behaviour options</param>
    public GenEmail(EmailGenOptions options)
    {
        Options = options;
    }

    public override string Generate(PCG pcg, Size? min, out Size size)
    {
        var localPart = CreateLocalPart(pcg, min, out _);
        var domainPart = CreateDomainPart(localPart, pcg, min, out size);

        return Options.PollutedEmailsEnabled
            ? CreatePollutedEmail(localPart, domainPart)
            : CreateEmail(localPart, domainPart);
    }

    /// <summary>
    /// Creates an email address based on the provided local and domain part.
    /// </summary>
    /// <param name="local">the localpart of the email address.</param>
    /// <param name="domain">the domainpart of the email address.</param>
    /// <returns></returns>
    private static string CreateEmail(string local, string domain)
    {
        return string.Join("@", local, domain);
    }

    /// <summary>
    /// Changes local or domain to produce an invalid email address. The way how the changes
    /// looks like depends on the length of the given local part.
    /// </summary>
    /// <param name="local">the localpart of the email address.</param>
    /// <param name="domain">the domainpart of the email address.</param>
    /// <returns></returns>
    private string CreatePollutedEmail(string local, string domain)
    {
        var caseNumber = local.Length % 4;

        var localIsQuoted = local.StartsWith('"') && local.EndsWith('"');
        var domainIsIpAddress = domain.StartsWith('[') && domain.EndsWith(']');

        switch (caseNumber)
        {
            case 0:
                local = OverExtendLocalPart(local);
                break;
            case 1:
                if (domainIsIpAddress)
                {
                    domain = OverExtendDomain(local, domain);
                }
                else
                {
                    local = OverExtendLocalPart(local);
                }
                break;
            case 2:
                var c = local.Length % 2 == 0 ? '-' : '.';
                local = localIsQuoted ? local + c : c + local;
                break;
            case 3:
                local = AddSpecialChars(local);
                break;
        }

        return CreateEmail(local, domain);
    }

    /// <summary>
    /// Adds additional signs to the local part to make it longer than 64 signs.
    /// </summary>
    /// <param name="local">the localpart of the email address.</param>
    /// <returns></returns>
    private static string OverExtendLocalPart(string local)
    {
        const byte localMaxLength = 64;
        var suffix = new string('x', localMaxLength - local.Length);

        // if local part is quotet the suffix must be inside the quotation marks
        var localIsQuoted = local.StartsWith('"') && local.EndsWith('"');
        
        return localIsQuoted
            ? local.Remove(local.Length - 1) + "." + suffix + "\""
            : string.Join(".", local, suffix);
    }

    /// <summary>
    /// Adds additional signs to the domain part to make it longer than the maximun permitted lenght.
    /// </summary>
    /// <param name="local">the localpart of the email address.</param>
    /// <param name="domain">the domainpart of the email address.</param>
    /// <returns></returns>
    private static string OverExtendDomain(string local, string domain)
    {
        var emailLength = local.Length + domain.Length + 1;
        var signsLeft = 256 - emailLength;

        return string.Join(".", domain, new string('z', signsLeft));
    }

    /// <summary>
    /// Adds some special characters at predefined positions.
    /// </summary>
    /// <param name="local">the localpart of the email address.</param>
    /// <returns></returns>
    private static string AddSpecialChars(string local)
    {
        var charArr = local.ToCharArray();
        charArr[local.Length / 2] = '\\';
        charArr[local.Length / 3] = '"';
        charArr[local.Length / 4] = ' ';
        return new string(charArr);
    }

    private string CreateLocalPart(PCG pcg, Size? min, out Size size)
    {
        var shouldBeQuoted = Options.QuotedLocalPartEnabled 
            ? genBool.Generate(pcg, min, out _)
            : false;

        string? local;
        if (shouldBeQuoted)
        {
            local = genLocalQuoted.Generate(pcg, min, out size);

            // backslashed must be escaped
            local = local.Replace("\\", "\\\\");

            // quotation marks inside the string are allowed and must be escaped
            local = local.Replace("\"", "\\\"");

            // set local part inside of quotation marks
            local = string.Format("\"{0}\"", local);
        }
        else
        {
            do
            {
                local = genLocal.Generate(pcg, min, out size);
            }
            while (LocalIsInvalid(local));
        }

        return local;
    }

    private string CreateDomainPart(string localPart, PCG pcg, Size? min, out Size size)
    {
        var shouldGenerateDomain = Options.IPv4Enabled 
            ? genBool.Generate(pcg, min, out _)
            : true;

        string? domain;
        if (shouldGenerateDomain)
        {
            domain = genDomain.Generate(pcg, min, out size);
            if (domain.Length + localPart.Length + 1 + 1 > 256)
            {
                // sanitize domain
                var maxLen = 255 - localPart.Length;
                domain = domain.Substring(maxLen);

                // maybe after remove a part from the 
                domain = DomainIsInvalid(domain) ? domain.Substring(1) : domain;
            }
        }
        else
        {
            var ip = genIpv4.Generate(pcg, min, out size);

            // ip addresses should be set into brackets
            domain = string.Format("[{0}]", ip.ToString());
        }

        return domain;
    }

    /// <summary>
    /// Checks if the domain is valid.
    /// </summary>
    /// <param name="domain">the domainpart of the email address.</param>
    /// <returns></returns>
    private static bool DomainIsInvalid(string domain) => domain.StartsWith('.') || domain.EndsWith('.');

    /// <summary>
    /// Checks if the local part ist valid.
    /// </summary>
    /// <param name="local">the localpart of the email address.</param>
    /// <returns></returns>
    private static bool LocalIsInvalid(string local) => local.StartsWith('.') || local.EndsWith('.') || local.Contains("..");
}
