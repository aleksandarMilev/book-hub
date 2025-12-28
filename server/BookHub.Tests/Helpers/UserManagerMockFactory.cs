namespace BookHub.Tests.Helpers;

using Microsoft.AspNetCore.Identity;
using Moq;

public static class UserManagerMockFactory
{
    public static Mock<UserManager<TUser>> Create<TUser>()
        where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();

        return new Mock<UserManager<TUser>>(
            store.Object,
            null!,
            null!,
            Array.Empty<IUserValidator<TUser>>(),
            Array.Empty<IPasswordValidator<TUser>>(),
            null!,
            null!,
            null!,
            null!
        );
    }
}
