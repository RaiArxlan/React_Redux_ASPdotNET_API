using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using React_Redux_ASPdotNET_API.Server.Interfaces;
using React_Redux_ASPdotNET_API.Server.Models;

namespace React_Redux_ASPdotNET_API.Server.Controllers
{
    /// <summary>
    /// Authentication controller for user login and logout.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenService tokenService;
        private readonly ILogger<AuthController> logger;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="tokenService"></param>
        /// <param name="logger"></param>
        public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService, ILogger<AuthController> logger, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.logger = logger;
            this.configuration = configuration;
        }

        /// <summary>
        /// Login method for user authentication.
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <response code="201">Login successful and token generated</response>
        /// <response code="401">Returns an unauthorized message</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] string Email, [FromForm] string Password)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                logger.LogWarning("Login attempt failed for non-existent user: {Email}", Email);
                return Unauthorized("Invalid user name or password!");
            }

            var signin = await userManager.CheckPasswordAsync(user, Password);
            if (!signin)
            {
                logger.LogWarning("Login attempt failed for user: {Email}", Email);
                return Unauthorized("Invalid user name or password!");
            }

            var (jwtToken, refreshToken) = await tokenService.GenerateJwtTokenAsync(Email);

            Response.Cookies.Append("JwtBasedCookie", jwtToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(configuration.GetValue("Jwt:CookieExpiryInDays", 1))
            });

            Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(configuration.GetValue("Jwt:CookieExpiryInDays", 1))
            });

            return Ok("Login Successful!!!");
        }

        /// <summary>
        /// Logout method for user authentication, clears the authentication cookie.
        /// </summary>
        /// <response code="200">User logout successful</response>
        /// <response code="401">User is unauthorized</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Cookies["JwtBasedCookie"];

            var email = await tokenService.GetEmailFromExpiredTokenAsync(token ?? string.Empty);

            Response.Cookies.Delete("JwtBasedCookie");
            Response.Cookies.Delete("RefreshToken");
            logger.LogInformation("User {Email} logged out successfully.", email ?? string.Empty);

            return Ok("Logout successful!!!");
        }

        /// <summary>
        /// Test method to check if the authorization works.
        /// </summary>
        /// <response code="200">Cookie is authorized</response>
        /// <response code="401">Cookie is unauthorized</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [HttpGet("test")]
        [Authorize]
        public IActionResult Test()
        {
            return Ok("Authorization works!!");
        }

        /// <summary>
        /// Refreshes the token for the user.
        /// </summary>
        /// <response code="200">Token refreshed</response>
        /// <response code="401">User is unauthorized</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [HttpPost("refreshtoken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {
            var previousRefreshToken = Request.Cookies["RefreshToken"];
            var expiredToken = Request.Cookies["JwtBasedCookie"];

            if (string.IsNullOrEmpty(previousRefreshToken) || string.IsNullOrEmpty(expiredToken))
                return Unauthorized("Invalid token");

            var email = await tokenService.GetEmailFromExpiredTokenAsync(expiredToken);

            if (string.IsNullOrEmpty(email))
                return Unauthorized("Invalid token");

            var user = await userManager.FindByEmailAsync(email);
            if (user == null || string.IsNullOrEmpty(user.Email))
                return Unauthorized("Invalid token");

            var isValid = await tokenService.ValidateRefreshTokenAsync(user.Email, previousRefreshToken);
            if (!isValid)
                return Unauthorized("Invalid token");

            var (jwtToken, refreshToken) = await tokenService.GenerateJwtTokenAsync(user.Email);

            Response.Cookies.Append("JwtBasedCookie", jwtToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(configuration.GetValue("Jwt:CookieExpiryInDays", 1))
            });

            Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(configuration.GetValue("Jwt:CookieExpiryInDays", 1))
            });

            logger.LogInformation("Token refreshed for user {Email}", email);

            return Ok("Token Refreshed");
        }
    }
}
