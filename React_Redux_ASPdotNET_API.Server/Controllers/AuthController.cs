using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace React_Redux_ASPdotNET_API.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] string Email, [FromForm] string Password)
        {
            var token = GenerateJwtToken(Email);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Ensure this is true in production
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("JWT:ExpirationMinutes"))
            };

            Response.Cookies.Append("JwtBasedCookie", token, cookieOptions);

            await Task.Delay(0);

            return Ok("Login Successfull!!!");
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("JwtBasedCookie");

            await Task.Delay(0);

            return Ok("Logout");
        }

        [HttpGet("test")]
        [Authorize]
        public IActionResult Test()
        {
            return Ok("Authorization works!!");
        }


        private string GenerateJwtToken(string email)
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

            return tokenHandler.WriteToken(token);
        }
    }
}
