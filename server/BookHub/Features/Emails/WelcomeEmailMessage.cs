namespace BookHub.Features.Emails;

// Holds only what the welcome email needs. Never add passwords or tokens here.
public sealed record WelcomeEmailMessage(
    string UserId,
    string Email,
    string Username,
    string BaseUrl);
