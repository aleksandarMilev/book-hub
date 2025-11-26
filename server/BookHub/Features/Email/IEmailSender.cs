namespace BookHub.Features.Email;

using Infrastructure.Services.ServiceLifetimes;

public interface IEmailSender : ITransientService
{
    Task SendWelcome(
        string email,
        string username);
}
