namespace BookHub.Features.Email
{
    using BookHub.Features.Email.Templates;
    using Infrastructure.Settings;
    using MailKit.Net.Smtp;
    using MailKit.Security;
    using Microsoft.Extensions.Options;
    using MimeKit;

    public class EmailSender(
        IOptions<EmailSettings> options,
        ILogger<EmailSender> logger) : IEmailSender
    {
        private readonly EmailSettings settings = options.Value;

        public async Task SendWelcome(
            string email,
            string username)
        {
            try
            {
                var body = WelcomeEmailTemplate.Build(username);

                await this.Send(
                   email,
                   "Welcome to BookHub 📚",
                   WelcomeEmailTemplate.Build(username));

                logger.LogInformation(
                    "User with email: {Email} and Username: {Username} successfully received email after registration",
                    email,
                    username);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to send welcome email to {Email}", email);
            }
        }

        private async Task Send(
            string to,
            string subject,
            string htmlBody,
            CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();

            message.From.Add(MailboxAddress.Parse(settings.From));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = htmlBody
            };

            using var client = new SmtpClient();

            try
            {
                var secureOption = settings.UseSsl
                    ? SecureSocketOptions.StartTls
                    : SecureSocketOptions.Auto;

                await client.ConnectAsync(
                    settings.Host,
                    settings.Port,
                    secureOption,
                    cancellationToken);

                if (!string.IsNullOrWhiteSpace(settings.Username))
                {
                    await client.AuthenticateAsync(
                        settings.Username,
                        settings.Password,
                        cancellationToken);
                }

                await client.SendAsync(message, cancellationToken);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Error sending email to {Recipient}", to);
                throw;
            }
            finally
            {
                if (client.IsConnected)
                {
                    await client.DisconnectAsync(true, cancellationToken);
                }
            }
        }
    }
}
