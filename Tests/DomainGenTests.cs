using CsCheck;
using CsCheck.Extension.Builder.Common;
using Xunit;

namespace Tests;

public class DomainGenTests
{
    [Fact]
    public void Max_Length_Is_Less_Or_Equal_255()
    {
        GenBuilder.Domain
            .Build()
            .Sample(domain => domain.Length <= 255);
    }

    [Fact]
    public void Has_One_Or_Multiple_Labels()
    {
        GenBuilder.Domain
            .Build()
            .Sample(domain =>
            {
                var labels = domain.Split('.').Where(part => !string.IsNullOrEmpty(part));
                return labels.Count() > 0;
            });
    }

    [Fact]
    public void Label_Max_Length_Is_Less_Or_Equal_63_Signs()
    {
        GenBuilder.Domain
            .Build()
            .Sample(domain =>
            {
                var labels = domain.Split('.').Where(part => part.Length <= 0 || 64 <= part.Length);
                return labels.Count() == 0;
            });
    }

    [Fact]
    public void Label_Consists_Of_Alphanumerical_And_Hyphens()
    {
        var hasInvalidCharacter = (string label) => label.ToList().Where(c => !char.IsLetterOrDigit(c) || c != '-');

        GenBuilder.Domain
            .Build()
            .Sample(domain =>
            {
                var labels = domain.Split('.').Select(label => hasInvalidCharacter(label));
                return labels.Count() > 0;
            });
    }

    [Fact]
    public void Label_Do_Not_Starts_Or_Ends_With_Hyphen()
    {
        GenBuilder.Domain
            .Build()
            .Sample(domain =>
            {
                var labels = domain.Split('.').Select(label => !label.StartsWith('-') || !label.EndsWith('-'));
                return labels.Count() > 0;
            });
    }

    [Fact]
    public void Domain_Has_At_Least_Two_Labels()
    {
        GenBuilder.Domain
            .DenyHostNames()
            .Build()
            .Sample(domain =>
            {
                var labels = domain.Split('.');
                return labels.Count() >= 2;
            });
    }
}
