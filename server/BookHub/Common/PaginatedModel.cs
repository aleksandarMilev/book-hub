namespace BookHub.Common;

public class PaginatedModel<T>(
    IEnumerable<T> items,
    int totalItems,
    int page,
    int pageSize)
{
    public IEnumerable<T> Items { get; init; } = items;

    public int TotalItems { get; init; } = totalItems;

    public int PageIndex { get; init; } = page;

    public int PageSize { get; init; } = pageSize;
}
