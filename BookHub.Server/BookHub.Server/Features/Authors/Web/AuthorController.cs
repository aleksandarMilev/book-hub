namespace BookHub.Server.Features.Authors.Web
{
    using Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Service;

    [Authorize]
    public class AuthorController(
        IAuthorService authorService,
        ICurrentUserService userService) : ApiController
    {
        private readonly IAuthorService authorService = authorService;
        private readonly ICurrentUserService userService = userService;
    }
}
