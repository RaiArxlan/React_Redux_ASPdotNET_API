using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using React_Redux_ASPdotNET_API.Server.Interfaces;
using React_Redux_ASPdotNET_API.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace React_Redux_ASPdotNET_API.Server.Services
{
    /// <inheritdoc />
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<TokenService> logger;

        /// <inheritdoc />
        public TokenService(IConfiguration configuration, UserManager<ApplicationUser> userManager, ILogger<TokenService> logger)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task<(string jwtToken, string refreshToken)> GenerateJwtTokenAsync(string email)
        {
            string audience = configuration.GetValue<string>("Jwt:Audience") ?? string.Empty;
            string issuer = configuration.GetValue<string>("Jwt:Issuer") ?? string.Empty;
            int expirationMinutes = configuration.GetValue<int>("Jwt:ExpirationMinutes");
            var key = Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key") ?? string.Empty);

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                logger.LogError("User not found for email {Email}", email);
                throw new Exception("User not found");
            }

            var roles = await userManager.GetRolesAsync(user);
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, email)
                };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                NotBefore = DateTime.UtcNow
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = tokenHandler.WriteToken(token);

            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            await userManager.UpdateAsync(user);

            return (jwtToken, HashToken(refreshToken));
        }

        /// <inheritdoc />
        public async Task<bool> ValidateRefreshTokenAsync(string email, string refreshToken)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                logger.LogError("User not found for email {Email}", email);
                return false;
            }

            return HashToken(user.RefreshToken) == refreshToken;
        }

        /// <inheritdoc/>
        public async Task<string> GetEmailFromExpiredTokenAsync(string token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = configuration.GetValue<string>("Jwt:Audience"),
                    ValidateIssuer = true,
                    ValidIssuer = configuration.GetValue<string>("Jwt:Issuer"),
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key") ?? string.Empty)),
                    ValidateLifetime = false // here we are saying that we don't care about the token's expiration date
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenValidation = await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);
                if (!tokenValidation.IsValid)
                {
                    logger.LogError("Token validation failed");

                    return string.Empty;
                }

                var emailClaim = tokenValidation?.Claims.FirstOrDefault(x => x.Key == ClaimTypes.Email);
                if (emailClaim == null || !emailClaim.HasValue || emailClaim.Value.Value == null || emailClaim.Value.Value is not string)
                {
                    logger.LogError("Email claim not found in token");

                    return string.Empty;
                }

                return (string)emailClaim.Value.Value;


            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting email from expired token.");
                return string.Empty;
            }
        }

        /// <summary>
        /// Generates a refresh token, a long random string :)
        /// </summary>
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Hashes a token using SHA256.
        /// </summary>
        private static string HashToken(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }
    }
}
