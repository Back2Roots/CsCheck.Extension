using CsCheck.Extension.Exceptions;
using CsCheck.Extension.Generators.Options;
using System.Net;

namespace CsCheck.Extension.Generators;

/// <summary>
/// Class <c>GenIPv4Address</c> generates random IPv4 addresses.
/// </summary>
public sealed class GenIPv4Address : Gen<IPAddress>
{
    private static readonly Gen<uint> genUInt = Gen.UInt;

    private IPv4GenOptions Options;

    /// <summary>
    /// Creates an IPv4 generator with default behaviour. Ignores all reserved IPv4 address areas per default.
    /// </summary>
    public GenIPv4Address()
    {
        Options = new IPv4GenOptions();
    }

    /// <summary>
    /// Creates an IPv4 generator with an customized generating behaviour.
    /// </summary>
    /// <param name="options">Alternative behaviour options</param>
    public GenIPv4Address(IPv4GenOptions options)
    {
        Options = options;
    }

    public override IPAddress Generate(PCG pcg, Size? min, out Size size)
    {
        int retryCounter = 0;
        uint value;
        do
        {
            if (retryCounter > Options.MaxGenRetries)
            {
                var message = "Maximum number of retries reached!";
                throw new GeneratorException(message);
            }

            value = genUInt.Generate(pcg, min, out size);
            retryCounter++;
        }
        while (IsInExcludedAddressArea(value) || IsHostOrBroadcastAddress(value) && Options.ExcludeHostAndBroadcast);

        return new IPAddress(value);
    }

    /// <summary>
    /// Checks if the given unsigned integer that represents an ipv4 address is in one of the
    /// reserved ip address areas that should be not part of the possible ip range.
    /// </summary>
    /// <param name="ipAsUInt">The ipv4 address as unsigned integer.</param>
    /// <returns>Returns true if the IPv4 address is inside of an excluded area, otherwise false.</returns>
    private bool IsInExcludedAddressArea(uint ipAsUInt)
    {
        foreach (var (key, range) in Options.ExcludedRanges)
        {
            if (range.min <= ipAsUInt && ipAsUInt <= range.max)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the given unsigned integer that represents an ipv4 address is a host or broadcast address
    /// of one of the 
    /// </summary>
    /// <param name="ipAsUInt">The ipv4 address as unsigned integer.</param>
    /// <returns>Returns true if the IPv4 address is a host or broadcast address.</returns>
    public static bool IsHostOrBroadcastAddress(uint ipAsUInt)
    {
        foreach (var (key, range) in IPv4GenOptions.ReservedAddressRanges)
        {
            if (ipAsUInt == range.min || ipAsUInt == range.max)
            {
                return true;
            }
        }

        if (ipAsUInt == 0 || ipAsUInt == UInt32.MaxValue)
        {
            return true;
        }
        return false;
    }
}
