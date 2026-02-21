namespace BookHub.Features.DataImporter.Service.Models
{
    public sealed record DataImportPartResult(
        string Entity,
        int TotalInFile,
        int Inserted,
        int SkippedExisting,
        int SkippedInvalid);
}
