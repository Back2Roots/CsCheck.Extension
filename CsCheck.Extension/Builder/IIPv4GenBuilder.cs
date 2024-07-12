using CsCheck.Extension.Builder.Common;
using CsCheck.Extension.Generators;

namespace CsCheck.Extension.Builder;

public interface IIPv4GenBuilder : IGenBuilder<GenIPv4Address>
{
    public IIPv4GenBuilder IncludePrivateNetworks();
    public IIPv4GenBuilder IncludeLoopback();
    public IIPv4GenBuilder IncludeTestNet();
    public IIPv4GenBuilder IncludeHostNet();
    public IIPv4GenBuilder IncludeReserved();
    public IIPv4GenBuilder IncludeSubnet();
    public IIPv4GenBuilder IncludeBroadcast();
    public IIPv4GenBuilder IncludeAnycast();
    public IIPv4GenBuilder IncludeBenchmark();
    public IIPv4GenBuilder IncludeAll();
    public IIPv4GenBuilder ExcludeAll();
    public IIPv4GenBuilder ExcludeHostAndBroadcastAddresses();
    public IIPv4GenBuilder DoRetries(int retries);
}
