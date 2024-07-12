using CsCheck.Extension.Builder.Common;
using CsCheck.Extension.Generators;
using CsCheck.Extension.Generators.Options;
using System.Net;
using System.Text.RegularExpressions;
using Xunit;

namespace CsCheck.Extension.Tests
{
    public class IPv4GenTests()
    {
        private static bool IsIPv4Address(string ipAddress)
        {
            var regEx = new Regex(@"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
            return regEx.IsMatch(ipAddress);
        }

        private static uint IPAddressToUInt(IPAddress address)
        {
            var octets = address.ToString().Split('.').Select(octet => Convert.ToUInt32(octet)).ToList();
            var asInt = 0U;
            asInt += octets[0] * (256 * 256 * 256);
            asInt += octets[1] * (256 * 256);
            asInt += octets[2] * 256;
            asInt += octets[3];
            return asInt;
        }

        [Fact]
        public void IPAddress_Is_Valid() 
        {
            GenBuilder.IPv4
                .Build()
                .Sample(ip => IsIPv4Address(ip.ToString()));
        }

        [Fact]
        public void IPAddress_Is_Not_Host_Or_Broadcast()
        {
            var options = new IPv4GenOptions();

            GenBuilder.IPv4
                .IncludeAll()
                .ExcludeHostAndBroadcastAddresses()
                .Build()
                .Sample(ip =>
                {
                    var ipAsUInt = IPAddressToUInt(ip);
                    return !GenIPv4Address.IsHostOrBroadcastAddress(ipAsUInt);
                });
        }
    }
}
