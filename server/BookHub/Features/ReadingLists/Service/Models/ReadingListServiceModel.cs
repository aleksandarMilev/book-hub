namespace BookHub.Features.ReadingLists.Service.Models;

using Shared;

public class ReadingListServiceModel
{
    public Guid BookId { get; init; }

    public ReadingListStatus Status { get; init; }
}
