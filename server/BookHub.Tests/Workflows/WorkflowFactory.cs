namespace BookHub.Tests.Workflows;

// The test authentication scheme (Create*Client) with the app's real AdminService: the admins
// are the users seeded with TestSeeder.SeedAdmin, so "notify every admin" is tested for real.
public sealed class WorkflowFactory : BookHubWebApplicationFactory
{
    protected override bool UseRealAdminService => true;
}
