namespace BookHub.Server.Features.Identity.Service
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    using static Constants;
    using static Common.Constants;

    public class IdentityService(
        UserManager<User> userManager,
        IOptions<AppSettings> appSettings) : IIdentityService
    {
        private readonly UserManager<User> userManager = userManager;
        private readonly AppSettings appSettings = appSettings.Value;

        public async Task<ResultWith<string>> RegisterAsync(string email, string username, string password)
        {
            var user = new User()
            {
                Email = email,
                UserName = username
            };

            var result = await this.userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var token = GenerateJwtToken(
                    this.appSettings.Secret,
                    user.Id,
                    user.UserName,
                    user.Email);

                return ResultWith<string>.Success(token);
            }

            var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));

            return ResultWith<string>.Failure(errorMessage ?? InvalidRegisterAttempt);
        }

        public async Task<ResultWith<string>> LoginAsync(string credentials, string password, bool rememberMe)
        {
            var user = await this.userManager.FindByNameAsync(credentials);

            if (user is null)
            {
                user = await this.userManager.FindByEmailAsync(credentials);

                if (user is null)
                {
                    return ResultWith<string>.Failure(InvalidLoginAttempt);
                }
            }

            if (await this.userManager.IsLockedOutAsync(user))
            {
                return ResultWith<string>.Failure(AccountIsLocked);
            }

            var passwordIsValid = await this.userManager.CheckPasswordAsync(user, password);

            if (passwordIsValid)
            {
                await this.userManager.ResetAccessFailedCountAsync(user);

                var isAdmin = await this.userManager.IsInRoleAsync(user, AdminRoleName);

                var token = GenerateJwtToken(
                    this.appSettings.Secret,
                    user.Id,
                    user.UserName!,
                    user.Email!,
                    rememberMe,
                    isAdmin);

                return ResultWith<string>.Success(token);
            }

            await this.userManager.AccessFailedAsync(user);

            if (await this.userManager.IsLockedOutAsync(user))
            {
                return ResultWith<string>.Failure(AccountWasLocked);
            }

            return ResultWith<string>.Failure(InvalidLoginAttempt);
        }

        private static string GenerateJwtToken(
            string appSettingsSecret,
            string userId,
            string username,
            string email,
            bool rememberMe = false,
            bool isAdmin = false)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettingsSecret);

            var claimList = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, userId),
                new(ClaimTypes.Name, username),
                new(ClaimTypes.Email, email)
            };

            if (isAdmin)
            {
                claimList.Add(new(ClaimTypes.Role, AdminRoleName));
            }

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claimList),
                Expires = rememberMe
                    ? DateTime.UtcNow.AddDays(ExtendedTokenExpirationTime)
                    : DateTime.UtcNow.AddDays(DefaultTokenExpirationTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
