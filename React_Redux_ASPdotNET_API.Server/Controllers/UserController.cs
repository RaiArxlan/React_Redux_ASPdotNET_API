using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using React_Redux_ASPdotNET_API.Server.Interfaces;
using React_Redux_ASPdotNET_API.Server.Models;

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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenService tokenService;
        private readonly ILogger<UserController> logger;

        /// <summary>
        /// Constructor for the user controller.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="userManager"></param>
        /// <param name="tokenService"></param>
        /// <param name="logger"></param>
        public UserController(IConfiguration configuration, UserManager<ApplicationUser> userManager, ITokenService tokenService, ILogger<UserController> logger)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.logger = logger;
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
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register([FromForm] string Email, [FromForm] string Password)
        {
            // Check if user already exists
            var userCheck = await userManager.FindByEmailAsync(Email);
            if (userCheck != null)
            {
                logger.LogWarning("Registration attempt failed: Email {Email} is already registered.", Email);
                return BadRequest("User email already registered");
            }

            var user = new ApplicationUser
            {
                Email = Email,
                UserName = Email
            };

            var userCreation = await userManager.CreateAsync(user);
            if (!userCreation.Succeeded)
            {
                logger.LogError("User creation failed for email {Email}: {Errors}", Email, userCreation.Errors);
                return BadRequest(userCreation.Errors);
            }

            var passwordCreation = await userManager.AddPasswordAsync(user, Password);
            if (!passwordCreation.Succeeded)
            {
                logger.LogError("Password creation failed for email {Email}: {Errors}", Email, passwordCreation.Errors);

                // Delete the user if password creation fails
                await userManager.DeleteAsync(user);

                return BadRequest(passwordCreation.Errors);
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

            logger.LogInformation("User {Email} registered successfully.", Email);
            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="Email"></param>
        /// <response code="204">User deleted successfully</response>
        /// <response code="404">User not found</response>
        [HttpDelete("{Email}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                logger.LogWarning("Delete attempt failed: User with email {Email} not found.", Email);
                return NotFound();
            }

            await userManager.DeleteAsync(user);
            logger.LogInformation("User {Email} deleted successfully.", Email);

            return NoContent();
        }

        //TODO: Add more endpoints for user management (e.g. update, delete, etc.)
    }
}
