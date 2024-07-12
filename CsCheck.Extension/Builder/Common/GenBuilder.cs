namespace CsCheck.Extension.Builder.Common;

/// <summary>
/// Builder for configure and construct a generator.
/// </summary>
public class GenBuilder
{
    public static IDomainGenBuilder Domain = new DomainGenBuilder();

    public static IIPv4GenBuilder IPv4 => new IPv4GenBuilder();

    public static IEmailGenBuilder Email => new EmailGenBuilder();
}
