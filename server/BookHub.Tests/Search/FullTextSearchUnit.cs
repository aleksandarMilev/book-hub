namespace BookHub.Tests.Search;

using FluentAssertions;

using static Infrastructure.Extensions.FullTextSearchExtensions;

public sealed class FullTextSearchUnit
{
    [Theory]
    [InlineData("harr", "harr:*")]
    [InlineData("  Harry   Potter ", "harry:* & potter:*")]
    [InlineData("ХАРИ пот", "хари:* & пот:*")]
    [InlineData("1984", "1984:*")]
    [InlineData("o'brien", "o:* & brien:*")]
    [InlineData("sci-fi", "sci:* & fi:*")]
    [InlineData("harry & !potter | (stone):*", "harry:* & potter:* & stone:*")]
    [InlineData("a<->b", "a:* & b:*")]
    [InlineData("x\\y", "x:* & y:*")]
    public void ToPrefixTsQuery_ShouldBuildAndedPrefixTerms_WithoutOperators(
        string input,
        string expected)
        => ToPrefixTsQuery(input).Should().Be(expected);

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("&|!():*")]
    [InlineData("' \" <-> \\")]
    public void ToPrefixTsQuery_ShouldReturnNull_WhenNoTermIsLeft(string? input)
        => ToPrefixTsQuery(input).Should().BeNull();
}
