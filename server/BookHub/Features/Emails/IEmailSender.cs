namespace BookHub.Features.Emails;

using Infrastructure.Services.ServiceLifetimes;

public interface IEmailSender : ITransientService
{
    Task SendWelcome(
        string userId,
        string email,
        string username);
}
