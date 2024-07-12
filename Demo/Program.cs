using CsCheck.Extension.Builder.Common;

namespace CsCheck.Extension.Demo;

internal class Demo
{
    static void Main(string[] args)
    {
        Console.WriteLine("Email address with default configuration!");
        Console.WriteLine(DefaultEmail());
        Console.WriteLine();

        Console.WriteLine("Email address can be possible generated with ip address as domain part!");
        Console.WriteLine(DefaultEmailWithDomainOrIP());
        Console.WriteLine();

        Console.WriteLine("Host or domain name with default configuration!");
        Console.WriteLine(DefaultHostOrDomain());
        Console.WriteLine();

        Console.WriteLine("Domain name with at least two labels!");
        Console.WriteLine(DefaultDomain());
        Console.WriteLine();

        Console.WriteLine("IPv4 address with default configuration");
        Console.WriteLine(DefaultIPv4());
        Console.WriteLine();


        Console.WriteLine("IPv4 address with included private adddress areas");
        Console.WriteLine(IPv4WithPrivateAddressAreas());
        Console.WriteLine();
    }

    private static string DefaultEmail()
    {
        return GenBuilder.Email.Build().Single();
    }

    private static string DefaultEmailWithDomainOrIP()
    {
        return GenBuilder.Email
            .AllowIPv4()
            .Build()
            .Single();
    }

    private static string DefaultHostOrDomain()
    {
        return GenBuilder.Domain
            .Build()
            .Single();
    }

    private static string DefaultDomain()
    {
        return GenBuilder.Domain
            .DenyHostNames()
            .Build()
            .Single();
    }


    private static string DefaultIPv4()
    {
        return GenBuilder.IPv4
            .Build()
            .Single()
            .ToString();
    }

    private static string IPv4WithPrivateAddressAreas()
    {
        return GenBuilder.IPv4
            .IncludeAll()
            .Build()
            .Single()
            .ToString();
    }
}
