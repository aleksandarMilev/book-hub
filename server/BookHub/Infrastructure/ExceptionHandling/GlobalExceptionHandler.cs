namespace BookHub.Infrastructure.ExceptionHandling;

using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

// The single exception handler, used by UseExceptionHandler() in every environment.
// Expected failures (ExpectedFailures) become a 4xx ProblemDetails. Everything else becomes
// a 500 ProblemDetails with a traceId. Exception details are added only in Development.
public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    IHostEnvironment environment,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public const string UnexpectedErrorDetail = "An unexpected error occurred.";

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        // The client went away: nobody reads the response, and it isn't a server error.
        if (exception is OperationCanceledException &&
            httpContext.RequestAborted.IsCancellationRequested)
        {
            logger.LogInformation(
                "Request aborted by the client. TraceId={TraceId}",
                traceId);

            httpContext.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;
            return true;
        }

        var problemDetails = new ProblemDetails();
        var expected = ExpectedFailures.Map(exception);

        if (expected is not null)
        {
            logger.LogWarning(
                exception,
                "Request failed with {StatusCode}: {ExceptionType}. TraceId={TraceId}",
                expected.StatusCode,
                exception.GetType().Name,
                traceId);

            problemDetails.Status = expected.StatusCode;
            problemDetails.Detail = expected.Detail;
        }
        else
        {
            logger.LogError(
                exception,
                "Unhandled exception. TraceId={TraceId}",
                traceId);

            problemDetails.Status = StatusCodes.Status500InternalServerError;
            problemDetails.Detail = UnexpectedErrorDetail;

            if (environment.IsDevelopment())
            {
                problemDetails.Extensions["exception"] = exception.ToString();
            }
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        return await problemDetailsService.TryWriteAsync(new()
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
            Exception = exception,
        });
    }
}
