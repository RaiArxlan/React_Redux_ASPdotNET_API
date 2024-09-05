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
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Product Description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Product Price
        /// </summary>
        public decimal Price { get; set; }
    }
}
