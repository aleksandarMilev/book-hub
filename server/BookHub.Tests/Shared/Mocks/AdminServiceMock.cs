namespace BookHub.Tests.Shared.Mocks;

using BookHub.Areas.Admin.Service;

public sealed class AdminServiceMock(params string[] adminIds) : IAdminService
{
    public Task<IReadOnlyCollection<string>> GetIds(
        CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyCollection<string>>(adminIds);
}
