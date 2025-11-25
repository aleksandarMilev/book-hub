namespace BookHub.Features.Identity.Service;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Data.Models;
using Infrastructure.Services.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using static Common.Constants.Names;
using static Shared.Constants;

public class IdentityService(
    UserManager<User> userManager,
    IOptions<ApplicationSettings> appSettings) : IIdentityService
{
    public async Task<ResultWith<string>> Register(
        string email,
        string username,
        string password)
    {
        var user = new User
        {
            Email = email,
            UserName = username
        };

        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            var token = this.GenerateJwtToken(
                appSettings.Value.Secret,
                user.Id,
                user.UserName,
                user.Email);

            return ResultWith<string>.Success(token);
        }

        var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));

        return ResultWith<string>.Failure(errorMessage ?? InvalidRegisterAttempt);
    }

    public async Task<ResultWith<string>> Login(
        string credentials,
        string password,
        bool rememberMe)
    {
        var user = await userManager.FindByNameAsync(credentials);
        if (user is null)
        {
            user = await userManager.FindByEmailAsync(credentials);
            if (user is null)
            {
                return ResultWith<string>.Failure(InvalidLoginAttempt);
            }
        }

        if (await userManager.IsLockedOutAsync(user))
        {
            return ResultWith<string>.Failure(AccountIsLocked);
        }

        var passwordIsValid = await userManager.CheckPasswordAsync(user, password);
        if (passwordIsValid)
        {
            await userManager.ResetAccessFailedCountAsync(user);

            var isAdmin = await userManager.IsInRoleAsync(user, AdminRoleName);

            var token = this.GenerateJwtToken(
                appSettings.Value.Secret,
                user.Id,
                user.UserName!,
                user.Email!,
                rememberMe,
                isAdmin);

            return ResultWith<string>.Success(token);
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

            Issuer = appSettings.Value.Issuer,
            Audience = appSettings.Value.Audience,

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
