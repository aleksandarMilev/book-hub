namespace BookHub.Features.Challenges.Data.Models;

using BookHub.Data.Models.Base;
using Identity.Data.Models;

public class ReadingCheckInDbModel : DeletableEntity<int>
{
    public string UserId { get; init; } = default!;

    public UserDbModel User { get; init; } = default!;

    public DateOnly Date { get; init; }
}
