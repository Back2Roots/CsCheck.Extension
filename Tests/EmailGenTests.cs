using CsCheck;
using CsCheck.Extension.Builder.Common;
using CsCheck.Extension.Generators;
using System.Text.RegularExpressions;
using Xunit;

namespace Tests;

public class EmailGenTests()
{
    private static string GetLocalPart(string email) => email.Substring(0, email.LastIndexOf('@'));

    private static string GetDomainPart(string email) => email.Substring(email.LastIndexOf("@") + 1);

    private static bool LocalPartIsQuoted(string localPart) => localPart.StartsWith('"') && localPart.EndsWith('"');

    private static bool DomainPartIsIPAddress(string domainPart)
    {
        var regEx = new Regex(@"^\[(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\]$");
        return regEx.IsMatch(domainPart);
    }

    private static bool CharIsValid(string email, string allowedSpecialChars)
    {
        foreach (char c in email)
        {
            if (!char.IsLetterOrDigit(c) && !allowedSpecialChars.Contains(c))
            {
                return false;
            }
        }
        return true;
    }

    [Fact]
    public void Email_Contains_At_Least_One_At_Sign()
    {
        var regex = new Regex("^.*[@].*$");
        GenBuilder.Email.Build().Sample(regex.IsMatch);
    }

    [Fact]
    public void Email_Max_Length_Is_Less_Then_256()
    {
        GenBuilder.Email.Build().Sample(email => email.Length < 256);
    }

    [Fact]
    public void LocalPart_Max_Length_Is_Less_Or_Equal_64()
    {
        var gen = new GenEmail();
        gen.Sample(email => GetLocalPart(email).Length <= 64);
    }

    [Fact]
    public void LocalPart_Do_Not_Start_Or_End_With_Dot_Sign()
    {
        GenBuilder.Email
            .AllowQuotedLocalPart()
            .Build()
            .Sample(email =>
            {
                var local = GetLocalPart(email);
                return !local.StartsWith('.') || !local.EndsWith('.');
            });
    }

    [Fact]
    public void Unquoted_LocalPart_Consists_Of_Alphanumerical_And_SpecialChars()
    {
        var allowCharacters = "!#$%&'*+-/=?^_`{|}~.";
        var gen = new GenEmail();
        gen.Sample(email => CharIsValid(GetLocalPart(email), allowCharacters));

    }

    [Fact]
    public void Unquoted_LocalPart_Do_Not_Have_Sequence_Of_Dot_Signs()
    {
        GenBuilder.Email
            .Build()
            .Sample(email =>
            {
                // check for ".." pattern
                var regEx = new Regex(@"\.{2}");
                var local = GetLocalPart(email);

                // test succeeds when regex do not match
                return !regEx.IsMatch(local);
            });
    }

    [Fact]
    public void Quoted_LocalPart_BackSlash_And_QuotationMark_Are_Escaped()
    {
        static bool isSpecialChar(char c) => c == '\\' || c == '"';

        var gen = new GenEmail();
        gen.Sample(email =>
        {
            var local = GetLocalPart(email);

            // test only applies to emails where localpart has been quoted
            if (!LocalPartIsQuoted(local))
            {
                return true;
            }

            // remove quotation marks at the start and end
            local = local.Substring(1, local.Length - 1);

            // check every character
            char prevChar = '0';
            foreach (char c in local)
            {
                // when c is a character that must be escaped the previous char
                // must be a backslash
                if (isSpecialChar(c) && prevChar != '\\')
                {
                    return false;
                }

                // set char as previous char at the end validation
                prevChar = c;
            }
            return true;
        });
    }

    [Fact]
    public void Quoted_LocalPart_Allows_More_SpecialChars()
    {
        var allowCharacters = "!#$%&'*+-/=?^_`{|}~\"(),:;<>@[\\]. ";
        GenBuilder.Email
            .AllowQuotedLocalPart()
            .Build()
            .Sample(email =>
            {
                var local = GetLocalPart(email);
                if (LocalPartIsQuoted(local))
                {
                    return CharIsValid(local, allowCharacters);
                }
                return true;
            });
    }


    [Fact]
    public void DomainPart_Is_Valid_IPAddress()
    {
        GenBuilder.Email
            .AllowIPv4()
            .Build()
            .Sample(email =>
            {
                var domainPart = GetDomainPart(email);

                // bracked covered domain parts are should be ip addresses
                if (domainPart.StartsWith('[') && domainPart.EndsWith(']'))
                {
                    var x = DomainPartIsIPAddress(domainPart);
                    return x;
                }

                // return true in every case where domain part is a domain or hostname
                return true;
            });
    }
}
