namespace BookHub.Infrastructure.Services.Result;

// What kind of failure a Result carries. Controllers map it to the HTTP status code
// (ControllerExtensions), so services never deal with HTTP directly.
public enum ErrorKind
{
    // 400: validation errors and invalid input. The default for `return "message";`.
    BadRequest,

    // 404: not found, or private content whose existence must not leak.
    NotFound,

    // 403: authenticated, but not allowed to modify this (public) resource.
    Forbidden,

    // 409: the request conflicts with the current state (duplicate, already exists).
    Conflict,
}
