namespace BookHub.Features.UserProfile.Web.Admin;

using Areas.Admin.Web;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Service;

using static Common.Constants.ApiRoutes;

public class ProfileController(IProfileService service) : AdminApiController
{
    [HttpDelete(Id)]
    public async Task<ActionResult> Delete(
        string id,
        CancellationToken token = default)
    {
        var result = await service.Delete(id, token);

        return this.NoContentOrBadRequest(result);
    }
}
