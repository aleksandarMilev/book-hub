namespace BookHub.Tests.Shared.Data;

public static class TestSeedFiles
{
    public static void EnsureSeedFileExists()
        => EnsureArticleSeedFileExists();

    private static void EnsureArticleSeedFileExists()
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "Features",
            "Articles",
            "Data",
            "Seed",
            "articles_seed.json");

        var directory = Path.GetDirectoryName(path)!;
        Directory.CreateDirectory(directory);

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "[]");
        }
    }
}
