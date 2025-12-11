namespace BookHub.Features.ReadingList.Service.Models;

using Data.Models;

public class ReadingListServiceModel
{
    public Guid BookId { get; init; }

    public ReadingListStatus Status { get; init; }
}
