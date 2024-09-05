using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using React_Redux_ASPdotNET_API.Server.Models;

namespace React_Redux_ASPdotNET_API.Server.Data
{

    /// <summary>
    /// Application database context.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        /// <summary>
        /// Default constructor, accepts options for the database setup
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        /// <summary>
        /// Products table
        /// </summary>
        public DbSet<Product> Products { get; set; }
    }
}