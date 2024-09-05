using Microsoft.IdentityModel.Tokens;
using React_Redux_ASPdotNET_API.Server.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace React_Redux_ASPdotNET_API.Server.Services
{
    /// <inheritdoc />
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        /// <inheritdoc />
        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        /// <inheritdoc />
        public async Task<string> GenerateJwtToken(string email)
        {
            string audience = configuration.GetValue<string>("Jwt:Audience") ?? string.Empty;
            string issuer = configuration.GetValue<string>("Jwt:Issuer") ?? string.Empty;
            int expirationMinutes = configuration.GetValue<int>("Jwt:ExpirationMinutes");
            var key = Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key") ?? string.Empty);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, "Admin"),
            };

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

            await Task.Delay(0);

            return jwtToken;
        }
    }
}
