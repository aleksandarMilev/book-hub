namespace BookHub.Features.Emails;

using Templates;
using Infrastructure.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

public class EmailSender(
    IOptions<EmailSettings> emailSettings,
    IOptions<AppUrlsSettings> appUrlsSettings,
    ILogger<EmailSender> logger) : IEmailSender
{
    public async Task SendWelcome(
        string userId,
        string email,
        string username)
    {
        try
        {
            var baseUrl = appUrlsSettings
                .Value
                .ClientBaseUrl?
                .TrimEnd('/')
                ?? throw new InvalidOperationException("AppUrlsSettings:ClientBaseUrl is not configured.");

            var body = WelcomeEmailTemplate.Build(
                username,
                baseUrl);

            await this.Send(
               email,
               "Welcome to BookHub 📚",
               body);

            logger.LogInformation(
                "Welcome email sent after registration. UserId={UserId}",
                userId);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Failed to send welcome email after registration. UserId={UserId}",
                userId);
        }
    }

    public async Task SendPasswordReset(
        string userId,
        string email,
        string resetUrl)
    {
        try
        {
            var body = PasswordResetEmailTemplate.Build(resetUrl);

            await this.Send(
                email,
                "Reset your BookHub password",
                body);

            logger.LogInformation(
                "Password reset email sent. UserId={UserId}",
                userId);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Failed to send password reset email. UserId={UserId}",
                userId);
        }
    }

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
