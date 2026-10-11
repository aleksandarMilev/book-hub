namespace BookHub.Infrastructure.Extensions;

using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NpgsqlTypes;

public static class FullTextSearchExtensions
{
    public const string SearchVectorProperty = "SearchVector";

    private const string TextSearchConfig = "simple";

    public static EntityTypeBuilder<T> HasSearchVector<T>(
        this EntityTypeBuilder<T> builder,
        params string[] includedPropertyNames)
        where T : class
    {
        builder
            .Property<NpgsqlTsVector>(SearchVectorProperty)
            .IsGeneratedTsVectorColumn(
                TextSearchConfig,
                includedPropertyNames);

        builder
            .HasIndex(SearchVectorProperty)
            .HasMethod("GIN");

        return builder;
    }

    public static IQueryable<T> ApplyFullTextSearch<T>(
        this IQueryable<T> query,
        string? searchTerm)
        where T : class
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return query;
        }

        var tsQuery = ToPrefixTsQuery(searchTerm);
        if (tsQuery is null)
        {
            return query.Where(_ => false);
        }

        return query.Where(e => EF
            .Property<NpgsqlTsVector>(e, SearchVectorProperty)
            .Matches(EF.Functions.ToTsQuery(TextSearchConfig, tsQuery)));
    }

    public static string? ToPrefixTsQuery(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        var terms = new List<string>();
        var current = new StringBuilder();

        foreach (var character in input)
        {
            if (char.IsLetterOrDigit(character))
            {
                current.Append(char.ToLowerInvariant(character));
                continue;
            }

            AddTerm(terms, current);
        }

        AddTerm(terms, current);

        return terms.Count == 0
            ? null
            : string.Join(" & ", terms);
    }

    private static void AddTerm(
        List<string> terms,
        StringBuilder current)
    {
        if (current.Length == 0)
        {
            return;
        }

        terms.Add($"{current}:*");
        current.Clear();
    }
}
