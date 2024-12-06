namespace BookHub.Server.Features.Identity.Service
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    using Microsoft.IdentityModel.Tokens;

    using static Common.Constants.Constants;

    public class IdentityService : IIdentityService
    {
        public string GenerateJwtToken(
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
