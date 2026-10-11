namespace BookHub.Tests.Shared.Utils;

using System.Net;
using System.Text.Json;
using FluentAssertions;

// Status assertions that print the response body when they fail, so a broken step in a long
// workflow shows the server's ProblemDetails instead of only the status code.
public static class ResponseAssertions
{
    public static async Task ShouldHaveStatus(
        this HttpResponseMessage response,
        HttpStatusCode expectedStatus)
    {
        var content = await response
            .Content
            .ReadAsStringAsync(TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(expectedStatus, content);
    }

    public static async Task<T> ShouldHaveBody<T>(
        this HttpResponseMessage response,
        HttpStatusCode expectedStatus = HttpStatusCode.OK)
    {
        var content = await response
            .Content
            .ReadAsStringAsync(TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(expectedStatus, content);

        var body = JsonSerializer.Deserialize<T>(content, JsonSerializerOptions.Web);
        body.Should().NotBeNull();

        return body!;
    }
}
