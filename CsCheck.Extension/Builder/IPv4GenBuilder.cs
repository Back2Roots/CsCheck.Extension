using CsCheck.Extension.Generators;
using CsCheck.Extension.Generators.Options;

namespace CsCheck.Extension.Builder;

/// <summary>
/// Generator for creating random IPv4 addresses. 
/// </summary>
public class IPv4GenBuilder : IIPv4GenBuilder
{
    private IPv4GenOptions Options { get; init; } = new IPv4GenOptions();

    public GenIPv4Address Build()
    {
        return new GenIPv4Address(Options);
    }

    public IIPv4GenBuilder ExcludeAll()
    {
        Options.ExcludeAll();
        return this;
    }

    public IIPv4GenBuilder IncludeAll()
    {
        Options.IncludeAll();
        return this;
    }

    public IIPv4GenBuilder IncludeAnycast()
    {
        Options.IncludeAnycast();
        return this;
    }

    public IIPv4GenBuilder IncludeBenchmark()
    {
        Options.IncludeBenchmark();
        return this;
    }

    public IIPv4GenBuilder IncludeBroadcast()
    {
        Options.IncludeBroadcast();
        return this;
    }

    public IIPv4GenBuilder IncludeHostNet()
    {
        Options.IncludeHostNet();
        return this;
    }

    public IIPv4GenBuilder IncludeLoopback()
    {
        Options.IncludeLoopback();
        return this;
    }

    public IIPv4GenBuilder IncludePrivateNetworks()
    {
        Options.IncludePrivateNetworks();
        return this;
    }

    public IIPv4GenBuilder IncludeReserved()
    {
        Options.IncludeReserved();
        return this;
    }

    public IIPv4GenBuilder IncludeSubnet()
    {
        Options.IncludeSubnet();
        return this;
    }

    public IIPv4GenBuilder IncludeTestNet()
    {
        Options.IncludeTestNet();
        return this;
    }

    public IIPv4GenBuilder DoRetries(int retries)
    {
        Options.DoRetries(retries);
        return this;
    }

    public IIPv4GenBuilder ExcludeHostAndBroadcastAddresses()
    {
        Options.ExcludeHostAndBroadcastAddresses();
        return this;
    }
}
