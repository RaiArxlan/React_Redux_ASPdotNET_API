using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using React_Redux_ASPdotNET_API.Server.Interfaces;

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
        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenService tokenService;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="userManager"></param>
        /// <param name="tokenService"></param>
        public AuthController(IConfiguration configuration, UserManager<IdentityUser> userManager, ITokenService tokenService)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.tokenService = tokenService;
        }

        /// <summary>
        /// Login method for user authentication.
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="401">Returns an unauthorized message</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] string Email, [FromForm] string Password)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null)
                return Unauthorized("Invalid user name or password!");

            var signin = await userManager.CheckPasswordAsync(user, Password);
            if (!signin)
                return Unauthorized("Invalid user name or password!");

            var token = await tokenService.GenerateJwtToken(Email);
            
            Response.Cookies.Append("JwtBasedCookie", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Ensure this is true in production
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("JWT:ExpirationMinutes"))
            });

            return Ok("Login Successfull!!!");
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
        public IActionResult Logout()
        {
            Response.Cookies.Delete("JwtBasedCookie");

            return Ok("Logout");
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
    }
}
