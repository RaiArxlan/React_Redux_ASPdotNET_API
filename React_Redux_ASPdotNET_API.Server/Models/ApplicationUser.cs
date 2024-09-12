using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace React_Redux_ASPdotNET_API.Server.Models
{
    /// <summary>
    /// Application user model.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Refresh token for the user.
        /// </summary>
        [StringLength(1000)]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
