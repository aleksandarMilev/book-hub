namespace BookHub.Server.Features
{
    public class PaginatedModel<T>(
        IEnumerable<T> items,
        int totalItems,
        int page,
        int pageSize)
    {
        public IEnumerable<T> Items { get; set; } = items;

        public int TotalItems { get; set; } = totalItems;

        public int Page { get; set; } = page;

        public int PageSize { get; set; } = pageSize;
    }
}
