namespace BookHub.Features.Emails;

using Templates;
using Infrastructure.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

public class EmailSender(
    IOptions<EmailSettings> emailSettings,
    ILogger<EmailSender> logger) : IEmailSender
{
    public async Task SendWelcome(
        string email,
        string username,
        string baseUrl,
        CancellationToken cancellationToken = default)
        => await this.Send(
            email,
            "Welcome to BookHub 📚",
            WelcomeEmailTemplate.Build(
                username,
                baseUrl),
            cancellationToken);

    public async Task SendPasswordReset(
        string email,
        string resetUrl,
        CancellationToken cancellationToken = default)
        => await this.Send(
            email,
            "Reset your BookHub password",
            PasswordResetEmailTemplate.Build(resetUrl),
            cancellationToken);

    private async Task Send(
        string to,
        string subject,
        string htmlBody,
        CancellationToken cancellationToken = default)
    {
        var message = new MimeMessage();

        message.From.Add(MailboxAddress.Parse(emailSettings.Value.From));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("html")
        {
            Text = htmlBody
        };

        using var client = new SmtpClient();

        try
        {
            var secureOption = emailSettings
                .Value
                .UseSsl
                    ? SecureSocketOptions.StartTls
                    : SecureSocketOptions.Auto;

            await client.ConnectAsync(
                emailSettings.Value.Host,
                emailSettings.Value.Port,
                secureOption,
                cancellationToken);

            if (!string.IsNullOrWhiteSpace(emailSettings.Value.Username))
            {
                await client.AuthenticateAsync(
                    emailSettings.Value.Username,
                    emailSettings.Value.Password,
                    cancellationToken);
            }

            await client.SendAsync(message, cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error sending email.");
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
