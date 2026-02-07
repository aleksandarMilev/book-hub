namespace BookHub.Common;

public class PaginatedModel<T> 
{
    // In unit tests we need this class with parametless constructor.
    // Use the one with parameters in the business logic
    private PaginatedModel()
    {
        this.Items = [];
        this.TotalItems = default;
        this.PageIndex = default;
        this.PageSize = default;
    }

    public PaginatedModel(
        IEnumerable<T> items,
        int totalItems,
        int pageIndex,
        int pageSize)
    {
        this.Items = items;
        this.TotalItems = totalItems;
        this.PageIndex = pageIndex;
        this.PageSize = pageSize;
    }

    public IEnumerable<T> Items { get; init; }

    public int TotalItems { get; init; }

    public int PageIndex { get; init; }

    public int PageSize { get; init; }
}
