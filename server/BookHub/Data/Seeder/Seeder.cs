namespace BookHub.Data.Seeder;

using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class Seeder
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static void SeedFromJson<TDbModel>(
        EntityTypeBuilder<TDbModel> builder,
        string featureFolder,
        string fileName)
        where TDbModel : class
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "Features",
            featureFolder,
            "Data",
            "Seed",
            fileName);

        if (!File.Exists(path))
        {
            return;
        }

        var json = File.ReadAllText(path);
        var data = JsonSerializer.Deserialize<List<TDbModel>>(json, Options) ?? [];

        builder.HasData(data);
    }
}
