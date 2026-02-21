namespace BookHub.Infrastructure.Services.PageClamper;

using ServiceLifetimes;

using static Common.Constants.DefaultValues;

public interface IPageClamper : ITransientService
{
    void ClampPageSizeAndIndex(
        ref int pageIndex,
        ref int pageSize,
        int maxPageSize = DefaultPageSize);
}
