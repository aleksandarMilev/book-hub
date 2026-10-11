namespace BookHub.Features.Identity.Service;

using BookHub.Data;
using Data.Models;
using Emails;
using Features.UserProfile.Service;
using Infrastructure.Services.Result;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using Shared;
using System.Text;
using static Common.Constants.Names;
using static Shared.Constants.ErrorMessages;

public class IdentityService(
    BookHubDbContext data,
    UserManager<UserDbModel> userManager,
    IEmailSender emailSender,
    IWelcomeEmailQueue welcomeEmailQueue,
    IProfileService profileService,
    ILogger<IdentityService> logger,
    IJwtTokenGenerator jwtTokenGenerator,
    IOptions<AppUrlsSettings> appUrlsSettings) : IIdentityService
{
    public async Task<ResultWith<string>> Register(
        RegisterServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        var normalizedUsername = userManager.NormalizeName(serviceModel.Username);
        var normalizedEmail = userManager.NormalizeEmail(serviceModel.Email);

        var usersQuery = userManager.Users;
        usersQuery = usersQuery.IgnoreQueryFilters();

        var usernameTaken = await usersQuery
            .AnyAsync(
                u => u.NormalizedUserName == normalizedUsername,
                cancellationToken);

        if (usernameTaken)
        {
            return ResultWith<string>.Conflict(
                $"Username '{serviceModel.Username}' is already taken.");
        }

        var emailTaken = await usersQuery
            .AnyAsync(
                u => u.NormalizedEmail == normalizedEmail,
                cancellationToken);

        if (emailTaken)
        {
            return ResultWith<string>.Conflict(
                $"Email '{serviceModel.Email}' is already taken.");
        }

        var user = new UserDbModel
        {
            Email = serviceModel.Email,
            UserName = serviceModel.Username,
            LockoutEnabled = true
        };

        IdentityResult identityResult;

        try
        {
            identityResult = await this.CreateUserAndProfile(
                user,
                serviceModel,
                cancellationToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            logger.LogError(exception,
                "Failed to complete registration. UserId={UserId}",
                user.Id);

            return ResultWith<string>.Failure(InvalidRegisterAttempt);
        }

        if (identityResult.Succeeded)
        {
            var jwt = jwtTokenGenerator.Generate(
                user.Id,
                serviceModel.Username,
                serviceModel.Email);

            logger.LogInformation("User successfully registered. UserId={UserId}", user.Id);

            // The welcome email is sent in the background and must never block or fail registration.
            var welcomeEmail = new WelcomeEmailMessage(
                user.Id,
                serviceModel.Email,
                serviceModel.Username,
                this.ClientBaseUrl);

            if (!welcomeEmailQueue.TryEnqueue(welcomeEmail))
            {
                logger.LogWarning(
                    "Welcome email queue is full; the email was not sent. UserId={UserId}",
                    user.Id);
            }

            return ResultWith<string>.Success(jwt);
        }

        var errorMessage = string.Join("; ", identityResult.Errors.Select(e => e.Description));

        return ResultWith<string>.Failure(errorMessage ?? InvalidRegisterAttempt);
    }

    public async Task<ResultWith<string>> Login(
        LoginServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager
            .FindByNameAsync(serviceModel.Credentials);

        if (user is null)
        {
            user = await userManager.FindByEmailAsync(
                serviceModel.Credentials);

            if (user is null)
            {
                return ResultWith<string>.Failure(InvalidLoginAttempt);
            }
        }

        if (user.IsDeleted)
        {
            return ResultWith<string>.Failure(InvalidLoginAttempt);
        }

        if (await userManager.IsLockedOutAsync(user))
        {
            return ResultWith<string>.Failure(AccountIsLocked);
        }

        var passwordIsValid = await userManager.CheckPasswordAsync(
            user,
            serviceModel.Password);

        if (passwordIsValid)
        {
            await userManager.ResetAccessFailedCountAsync(user);

            var isAdmin = await userManager.IsInRoleAsync(user, AdminRoleName);
            var jwt = jwtTokenGenerator.Generate(
                user.Id,
                user.UserName!,
                user.Email!,
                serviceModel.RememberMe,
                isAdmin);

            return ResultWith<string>.Success(jwt);
        }

        await userManager.AccessFailedAsync(user);

        if (await userManager.IsLockedOutAsync(user))
        {
            return ResultWith<string>.Failure(AccountWasLocked);
        }

        return ResultWith<string>.Failure(InvalidLoginAttempt);
    }

    public async Task<ResultWith<string>> ForgotPassword(
        ForgotPasswordServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        const string GenericMessage =
            "If an account exists for that email, a password reset link has been sent.";

        var user = await userManager.FindByEmailAsync(serviceModel.Email);

        if (user is null || user.IsDeleted)
        {
            return ResultWith<string>.Success(GenericMessage);
        }

        try
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(token));

            var resetPath = $"{this.ClientBaseUrl}/identity/reset-password";
            var resetUrl = QueryHelpers.AddQueryString(
                resetPath,
                new Dictionary<string, string?>
                {
                    ["email"] = user.Email,
                    ["token"] = encodedToken
                });

            await emailSender.SendPasswordReset(
                user.Email!,
                resetUrl,
                cancellationToken);

            return ResultWith<string>.Success(GenericMessage);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Failed generating/sending password reset email. UserId={UserId}",
                user.Id);

            return ResultWith<string>.Success(GenericMessage);
        }
    }

    public async Task<ResultWith<string>> ResetPassword(
        ResetPasswordServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(serviceModel.Email);

        if (user is null || user.IsDeleted)
        {
            return ResultWith<string>.Failure(InvalidPasswordResetAttempt);
        }

        string token;

        try
        {
            var tokenBytes = WebEncoders.Base64UrlDecode(serviceModel.Token);
            token = Encoding.UTF8.GetString(tokenBytes);
        }
        catch
        {
            return ResultWith<string>.Failure(InvalidPasswordResetAttempt);
        }

        var result = await userManager.ResetPasswordAsync(
            user,
            token,
            serviceModel.NewPassword);

        if (!result.Succeeded)
        {
            var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
            return ResultWith<string>.Failure(errorMessage ?? InvalidPasswordResetAttempt);
        }

        return ResultWith<string>.Success("Password successfully reset.");
    }

    // The user and the profile commit together or not at all (B-02). A user left behind by a
    // failed profile creation would keep its username and email taken, because the uniqueness
    // checks include soft-deleted users. UserManager saves through the same scoped DbContext,
    // so the transaction covers both. Retry-on-failure is configured, so the transaction runs
    // inside the execution strategy.
    private async Task<IdentityResult> CreateUserAndProfile(
        UserDbModel user,
        RegisterServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        var strategy = data.Database.CreateExecutionStrategy();

        try
        {
            return await strategy.ExecuteAsync(async () =>
            {
                // A retried attempt starts from a clean tracker (the rolled-back rows are gone).
                data.ChangeTracker.Clear();

                await using var transaction = await data
                    .Database
                    .BeginTransactionAsync(cancellationToken);

                var identityResult = await userManager.CreateAsync(
                    user,
                    serviceModel.Password);

                if (!identityResult.Succeeded)
                {
                    return identityResult;
                }

                await profileService.Create(
                    serviceModel.ToCreateProfileServiceModel(),
                    user.Id,
                    cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return identityResult;
            });
        }
        catch
        {
            // The transaction was rolled back, so nothing tracked here exists any more.
            data.ChangeTracker.Clear();
            throw;
        }
    }

    // Required and validated as a URL on startup. Trimmed so "http://x/" doesn't produce "//identity/...".
    private string ClientBaseUrl
        => appUrlsSettings
            .Value
            .ClientBaseUrl
            .TrimEnd('/');
}
