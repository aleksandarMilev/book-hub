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
        string path)
        where TDbModel : class
    {
        if (!File.Exists(path))
        {
            return;
        }

        var json = File.ReadAllText(path);
        var data = JsonSerializer.Deserialize<List<TDbModel>>(json, Options) ?? [];

        builder.HasData(data);
    }
}
