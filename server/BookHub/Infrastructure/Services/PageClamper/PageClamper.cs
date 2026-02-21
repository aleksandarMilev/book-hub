namespace BookHub.Infrastructure.Services.PageClamper;

using static Common.Constants.DefaultValues;

public class PageClamper : IPageClamper
{
    public void ClampPageSizeAndIndex(
        ref int pageIndex,
        ref int pageSize,
        int maxPageSize = DefaultPageSize)
    {
        pageIndex = Math.Max(pageIndex, DefaultPageIndex);
        pageSize = Math.Clamp(pageSize, 1, maxPageSize);
    }
}
