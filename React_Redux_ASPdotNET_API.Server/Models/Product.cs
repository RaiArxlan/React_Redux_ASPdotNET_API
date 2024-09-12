using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace React_Redux_ASPdotNET_API.Server.Models
{
    /// <summary>
    /// Product class for the API model
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Product ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product Name
        /// </summary>
        [StringLength(100, ErrorMessage = "Maximum 100 characters allowed!")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Product Description
        /// </summary>
        [StringLength(100, ErrorMessage = "Maximum 100 characters allowed!")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Product Price
        /// </summary>
        [Precision(18, 2)]
        public decimal Price { get; set; }
    }
}
