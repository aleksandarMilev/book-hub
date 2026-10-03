namespace BookHub.Tests.Shared.Utils;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

// Every error response is an RFC 9457 ProblemDetails with a traceId (Phase 2b).
public static class ProblemDetailsAssertions
{
    public static async Task<ProblemDetails> ShouldBeProblem(
        this HttpResponseMessage response,
        HttpStatusCode expectedStatus,
        string? expectedDetail = null)
    {
        response.StatusCode.Should().Be(expectedStatus);

        response
            .Content
            .Headers
            .ContentType?
            .MediaType
            .Should()
            .Be("application/problem+json");

        var problem = await response
            .Content
            .ReadFromJsonAsync<ProblemDetails>();

        problem.Should().NotBeNull();
        problem!.Status.Should().Be((int)expectedStatus);
        problem.Extensions.Should().ContainKey("traceId");

        if (expectedDetail is not null)
        {
            problem.Detail.Should().Be(expectedDetail);
        }

        return problem;
    }
}
