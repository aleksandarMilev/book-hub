namespace BookHub.Tests.Shared.Mocks;

using BookHub.Areas.Admin.Service;

public sealed class AdminServiceMock(string adminId) : IAdminService
{
    public Task<string> GetId()
        => Task.FromResult(adminId);
}