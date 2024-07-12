namespace CsCheck.Extension.Generators.Options;

/// <summary>
/// Options that should affect IPv4 generator behaviour.
/// </summary>
public class IPv4GenOptions
{
    public Dictionary<string, (uint min, uint max)> ExcludedRanges { get; private set; } = [];
    public static IReadOnlyDictionary<string, (uint min, uint max)> ReservedAddressRanges { get; private set; } =
        new Dictionary<string, (uint min, uint max)>()
        {
            { "host", (0U, 16777215U) },
            { "Anycast", (3227017984U, 3227018239U) },
            { "Loopback", (2130706432U, 2147483647U) },
            { "PrivateNetwork#1", (167772160U, 184549375U) },
            { "PrivateNetwork#2", (2886729728U, 2887778303U) },
            { "PrivateNetwork#3", (3232235520U, 3232301055U) },
            { "TEST-NET#1", (3221225984U, 3221226239U) },
            { "TEST-NET#2", (3325256704U, 3325256959U) },
            { "TEST-NET#3", (3405803776U, 3405804031U) },
            { "Reserved#1", (3221225472U, 3221225727U) },
            { "Reserved#2", (3925606400U, 3925606655U) },
            { "Subnet", (2851995648U, 2852061183U) },
            { "Benchmark", (3323068416U, 3323199487U) },
            { "Broadcast", (4294967295U, 4294967295U) }
        };

    public bool ExcludeHostAndBroadcast { get; private set; } = false;

    public int MaxGenRetries { get; private set; } = 50;

    public IPv4GenOptions() 
    {
        InitReservedBlocks();
    }

    /// <summary>
    /// Includes private network address blocks
    ///     - 10.0.0.0/8
    ///     - 172.16.0.0/12
    ///     - 192.168.0.0/16
    /// </summary>
    public IPv4GenOptions IncludePrivateNetworks()
    {
        ExcludedRanges.Remove("PrivateNetwork#1");
        ExcludedRanges.Remove("PrivateNetwork#2");
        ExcludedRanges.Remove("PrivateNetwork#3");
        return this;
    }

    /// <summary>
    /// Includes loopback address blocks
    ///     - 127.0.0.0/8
    /// </summary>
    public IPv4GenOptions IncludeLoopback()
    {
        ExcludedRanges.Remove("Loopback");
        return this;
    }

    /// <summary>
    /// Includes Test-Net address blocks provided for documentation purposes
    ///     - 192.0.2.0/24
    ///     - 198.51.100.0/24
    ///     - 203.0.113.0/24
    /// </summary>
    public IPv4GenOptions IncludeTestNet()
    {
        ExcludedRanges.Remove("TEST-NET#1");
        ExcludedRanges.Remove("TEST-NET#2");
        ExcludedRanges.Remove("TEST-NET#3");
        return this;
    }

    /// <summary>
    /// Includes host net address block
    ///     - 0.0.0.0/8
    /// </summary>
    public IPv4GenOptions IncludeHostNet()
    {
        ExcludedRanges.Remove("Host");
        return this;
    }

    /// <summary>
    /// Includes reserved address blocks
    ///     - 192.0.0.0/24
    ///     - 240.0.0.0/4
    /// </summary>
    public IPv4GenOptions IncludeReserved()
    {
        ExcludedRanges.Remove("Reserved#1");
        ExcludedRanges.Remove("Reserved#2");
        return this;
    }

    /// <summary>
    /// Includes subnet address blocks
    ///     - 169.254.0.0/16
    /// </summary>
    public IPv4GenOptions IncludeSubnet()
    {
        ExcludedRanges.Remove("Subnet");
        return this;
    }

    /// <summary>
    /// Includes broadcast address blocks
    ///     - 255.255.255.255/32
    /// </summary>
    public IPv4GenOptions IncludeBroadcast()
    {
        ExcludedRanges.Remove("Broadcast");
        return this;
    }


    /// <summary>
    /// Includes anycast address blocks
    ///     - 192.88.99.0/24
    /// </summary>
    public IPv4GenOptions IncludeAnycast()
    {
        ExcludedRanges.Remove("Anycast");
        return this;
    }

    /// <summary>
    /// Includes benchmark address blocks
    ///     - 198.18.0.0/15
    /// </summary>
    public IPv4GenOptions IncludeBenchmark()
    {
        ExcludedRanges.Remove("Benchmark");
        return this;
    }

    /// <summary>
    /// Includes all reserved address blocks
    /// </summary>
    public IPv4GenOptions IncludeAll()
    {
        ExcludedRanges.Clear();
        return this;
    }

    /// <summary>
    /// Excludes all reserved address blocks
    /// </summary>
    public IPv4GenOptions ExcludeAll()
    {
        InitReservedBlocks();
        return this;
    }

    /// <summary>
    /// Adds all reserved ip address blocks to the exclude list
    /// </summary>
    private void InitReservedBlocks()
    {
        ExcludedRanges = ReservedAddressRanges.ToDictionary();
    }

    /// <summary>
    /// Exckudes the host and broadcast address of an address area.
    /// </summary>
    /// <returns></returns>
    public IPv4GenOptions ExcludeHostAndBroadcastAddresses()
    {
        ExcludeHostAndBroadcast = true;
        return this;
    }

    /// <summary>
    /// The number of retries if the generated value belongs to an include
    /// reserved area.
    /// Throws an exception if the maximum number of tries are reached.
    /// </summary>
    /// <param name="retries"></param>
    public IPv4GenOptions DoRetries(int retries)
    {
        MaxGenRetries = retries;
        return this;
    }
}
