namespace BookHub.Features.Identity.Service;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Data.Models;
using Email;
using Features.UserProfile.Service;
using Infrastructure.Services.Result;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using Shared;

using static Common.Constants.Names;
using static Shared.Constants.ErrorMessages;
using static Shared.Constants.TokenExpiration;

public class IdentityService(
    UserManager<UserDbModel> userManager,
    IEmailSender emailSender,
    IProfileService profileService,
    ILogger<IdentityService> logger,
    IOptions<JwtSettings> settings) : IIdentityService
{
    private readonly JwtSettings settings = settings.Value;

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
            return ResultWith<string>.Failure(
                $"Username '{serviceModel.Username}' is already taken.");
        }

        var emailTaken = await usersQuery
            .AnyAsync(
                u => u.NormalizedEmail == normalizedEmail,
                cancellationToken);

        if (emailTaken)
        {
            return ResultWith<string>.Failure(
                $"Email '{serviceModel.Email}' is already taken.");
        }

        var user = new UserDbModel
        {
            Email = serviceModel.Email,
            UserName = serviceModel.Username
        };

        var identityResult = await userManager.CreateAsync(
            user,
            serviceModel.Password);

        if (identityResult.Succeeded)
        {
            try
            {
                var jwt = this.GenerateJwtToken(
                    this.settings.Secret,
                    user.Id,
                    serviceModel.Username,
                    serviceModel.Email);

                logger.LogInformation(
                    "User with email: {Email} and Username: {Username} successfully registered.",
                    serviceModel.Email,
                    serviceModel.Username);

                await profileService.Create(
                    serviceModel.ToCreateProfileServiceModel(),
                    user.Id,
                    cancellationToken);

                await emailSender.SendWelcome(
                    serviceModel.Email,
                    serviceModel.Username);

                return ResultWith<string>.Success(jwt);
            }
            catch (Exception exception)
            {
                logger.LogError(exception,
                    "Failed to complete registration for {Email}",
                    serviceModel.Email);

                await userManager.DeleteAsync(user);

                return ResultWith<string>.Failure(InvalidRegisterAttempt);
            }
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
            var jwt = this.GenerateJwtToken(
                this.settings.Secret,
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

    private string GenerateJwtToken(
        string appSettingsSecret,
        string userId,
        string username,
        string email,
        bool rememberMe = false,
        bool isAdmin = false)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.OutboundClaimTypeMap.Clear();

        var key = Encoding.ASCII.GetBytes(appSettingsSecret);

        var claimList = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Name, username),
            new(ClaimTypes.Email, email)
        };

        if (isAdmin)
        {
            claimList.Add(new(ClaimTypes.Role, AdminRoleName));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claimList),
            Expires = rememberMe
                ? DateTime.UtcNow.AddDays(ExtendedTokenExpirationTime)
                : DateTime.UtcNow.AddDays(DefaultTokenExpirationTime),
            Issuer = this.settings.Issuer,
            Audience = this.settings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
