namespace BookHub.Infrastructure.Services.ImageWriter;

// An uploaded image failed validation. The global exception handler maps it to a 400
// ProblemDetails with this message, so it never surfaces as a 500.
public sealed class ImageValidationException(string message) : Exception(message);
