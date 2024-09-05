using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using React_Redux_ASPdotNET_API.Server.Interfaces;

namespace React_Redux_ASPdotNET_API.Server.Controllers
{
    /// <summary>
    /// User controller for user registration and management.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenService tokenService;

        /// <summary>
        /// Constructor for the user controller.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="userManager"></param>
        /// <param name="tokenService"></param>
        public UserController(IConfiguration configuration, UserManager<IdentityUser> userManager, ITokenService tokenService)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.tokenService = tokenService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="Email">The email address of the user.</param>
        /// <param name="Password">The password for the user.</param>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="400">If the user email is already registered or there were errors in user creation or password addition</response>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IdentityUser), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register([FromForm] string Email, [FromForm] string Password)
        {
            // Check if user already exists
            var userCheck = await userManager.FindByEmailAsync(Email);
            if (userCheck != null)
            {
                return BadRequest("User email already registered");
            }

            var user = new IdentityUser
            {
                Email = Email,
                UserName = Email
            };
            var userCreation = await userManager.CreateAsync(user);
            if (!userCreation.Succeeded)
            {
                return BadRequest(userCreation.Errors);
            }

            var passwordCreation = await userManager.AddPasswordAsync(user, Password);
            if (!passwordCreation.Succeeded)
            {
                return BadRequest(passwordCreation.Errors);
            }

            // Everything is okay
            var token = await tokenService.GenerateJwtToken(Email);

            Response.Cookies.Append("JwtBasedCookie", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Ensure this is true in production
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("JWT:ExpirationMinutes"))
            });

            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
        }

        //TODO: Add more endpoints for user management (e.g. update, delete, etc.)

    }
}
