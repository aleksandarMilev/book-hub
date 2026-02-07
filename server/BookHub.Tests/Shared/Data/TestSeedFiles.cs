namespace BookHub.Tests.Shared.Data;

public static class TestSeedFiles
{
    public static void EnsureSeedFileExists()
    {
        CheckSeedFileExists("Articles");
        CheckSeedFileExists("Authors");
    }

    private static void CheckSeedFileExists(string featureName)
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "Features",
            featureName,
            "Data",
            "Seed",
            $"{featureName.ToLower()}_seed.json");

        var directory = Path.GetDirectoryName(path)!;
        Directory.CreateDirectory(directory);

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "[]");
        }
    }
}
