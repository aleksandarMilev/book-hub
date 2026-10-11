namespace BookHub.Tests.Hosting;

using System.Net;
using System.Net.Http.Json;
using Common;
using Features.Books.Service.Models;
using Features.Notifications.Service.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Shared.Utils;

using static Common.Constants.Names;

// What the host does outside Development, around the endpoints: the startup admin bootstrap
// (B-01), CORS from configuration, and the per-IP rate limiter. The "Testing" environment runs
// the same pipeline as Production here.
public sealed class HostingIntegration : IAsyncLifetime
{
    private readonly BookHubWebApplicationFactory factory = new();

    public async ValueTask InitializeAsync()
        => await this.factory.ResetDatabase();

    public ValueTask DisposeAsync()
    {
        this.factory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task BootstrapAdmin_ShouldBeCreatedOnStartup_LogIn_AndReceiveTheAdminNotifications()
    {
        await using var bootstrap = new BootstrapAdminFactory();
        await bootstrap.ResetDatabase();

        // 1. The admin exists after startup and logs in with the configured credentials. Its
        //    token carries the role: an admin-only endpoint answers 404 for a missing book, not 403.
        var admin = await bootstrap.LoginAndAuthenticate(BootstrapAdminFactory.AdminEmail, BootstrapAdminFactory.Password);

        await (await admin.GetAsync($"/Administrator/Books/{Guid.NewGuid()}/", TestContext.Current.CancellationToken))
            .ShouldBeProblem(HttpStatusCode.NotFound);

        // 2. It's an admin to the rest of the app too: a reader's new book notifies it, and it approves.
        var reader = await bootstrap.RegisterAndAuthenticate("reader");

        var book = await (await reader.PostAsync("/Books", Forms.Book("A first book"), TestContext.Current.CancellationToken))
            .ShouldHaveBody<BookDetailsServiceModel>(HttpStatusCode.Created);

        var notifications = await (await admin.GetAsync("/Notifications/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<PaginatedModel<NotificationServiceModel>>();

        notifications.Items.Should().ContainSingle(n => n.ResourceId == book.Id);

        await (await admin.PatchAsync($"/Administrator/Books/{book.Id}/approve/", null, TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        // 3. A restart on the same database finds the role, the user and the membership in place.
        using (var restarted = bootstrap.WithWebHostBuilder(_ => { }))
        {
            await (await restarted.CreateClient().GetAsync("/health", TestContext.Current.CancellationToken))
                .ShouldHaveStatus(HttpStatusCode.OK);
        }

        var (admins, roles) = await bootstrap.WithData(async data => (
            await data.Users.CountAsync(u => u.Email == BootstrapAdminFactory.AdminEmail, TestContext.Current.CancellationToken),
            await data.Roles.CountAsync(r => r.Name == AdminRoleName, TestContext.Current.CancellationToken)));

        admins.Should().Be(1);
        roles.Should().Be(1);

        await (await bootstrap.CreateClient().PostAsJsonAsync(
                "/Identity/login/",
                new { credentials = BootstrapAdminFactory.AdminEmail, password = BootstrapAdminFactory.Password, rememberMe = false },
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.OK);
    }

    [Fact]
    public async Task BootstrapAdmin_ShouldNotCreateAnything_WhenItIsNotEnabled()
    {
        // The default factory leaves BootstrapAdmin:Enabled unset.
        var (users, adminRoles) = await this.factory.WithData(async data => (
            await data.Users.CountAsync(TestContext.Current.CancellationToken),
            await data.Roles.CountAsync(r => r.Name == AdminRoleName, TestContext.Current.CancellationToken)));

        users.Should().Be(0);
        adminRoles.Should().Be(0);
    }

    [Fact]
    public async Task Cors_ShouldAllowTheConfiguredOrigin_AndNoOther()
    {
        var httpClient = this.factory.CreateAnonymousClient();

        var allowed = await httpClient.SendAsync(Preflight(BookHubWebApplicationFactory.AllowedOrigin), TestContext.Current.CancellationToken);

        allowed.StatusCode.Should().Be(HttpStatusCode.NoContent);
        allowed.Headers.GetValues("Access-Control-Allow-Origin").Should().Equal(BookHubWebApplicationFactory.AllowedOrigin);
        allowed.Headers.GetValues("Access-Control-Allow-Methods").Should().Contain("POST");

        var other = await httpClient.SendAsync(Preflight("https://evil.example"), TestContext.Current.CancellationToken);

        other.Headers.Contains("Access-Control-Allow-Origin").Should().BeFalse();

        // A simple request from the client's origin is answered with the header too.
        using var request = new HttpRequestMessage(HttpMethod.Get, "/Genres/");
        request.Headers.Add("Origin", BookHubWebApplicationFactory.AllowedOrigin);

        var get = await httpClient.SendAsync(request, TestContext.Current.CancellationToken);

        get.StatusCode.Should().Be(HttpStatusCode.OK);
        get.Headers.GetValues("Access-Control-Allow-Origin").Should().Equal(BookHubWebApplicationFactory.AllowedOrigin);
    }

    [Fact]
    public async Task RateLimiter_ShouldAnswerWithATooManyRequestsProblem_OverTheLimit()
    {
        // 240 requests a minute per IP outside Development; this host's window is its own.
        const int Limit = 240;

        var httpClient = this.factory.CreateAnonymousClient();

        for (var sent = 0; sent < Limit; sent += 20)
        {
            var batch = await Task.WhenAll(Enumerable
                .Range(0, 20)
                .Select(_ => httpClient.GetAsync("/health", TestContext.Current.CancellationToken)));

            batch.Should().OnlyContain(r => r.StatusCode == HttpStatusCode.OK);
        }

        var rejected = await httpClient.GetAsync("/health", TestContext.Current.CancellationToken);

        var problem = await rejected.ShouldBeProblem(HttpStatusCode.TooManyRequests, "Too many requests. Try again later.");

        problem.Title.Should().Be("Too Many Requests");

        var retryAfter = int.Parse(rejected.Headers.GetValues("Retry-After").Single(), System.Globalization.CultureInfo.InvariantCulture);
        retryAfter.Should().BeInRange(1, 60);
    }

    private static HttpRequestMessage Preflight(string origin)
    {
        var request = new HttpRequestMessage(HttpMethod.Options, "/Books");
        request.Headers.Add("Origin", origin);
        request.Headers.Add("Access-Control-Request-Method", "POST");
        request.Headers.Add("Access-Control-Request-Headers", "authorization");

        return request;
    }

    // BootstrapAdmin as Compose sets it for production (BOOTSTRAP_ADMIN_*).
    private sealed class BootstrapAdminFactory : BookHubWebApplicationFactory
    {
        public const string AdminEmail = "owner@bookhub.test";
        public const string Password = "Adm1nPassw0rd";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder
                .UseSetting("BootstrapAdmin:Enabled", "true")
                .UseSetting("BootstrapAdmin:Email", AdminEmail)
                .UseSetting("BootstrapAdmin:Password", Password)
                .UseSetting("BootstrapAdmin:Role", AdminRoleName);
        }
    }
}
