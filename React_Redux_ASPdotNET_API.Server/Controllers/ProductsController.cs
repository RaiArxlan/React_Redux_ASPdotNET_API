using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using React_Redux_ASPdotNET_API.Server.Data;
using React_Redux_ASPdotNET_API.Server.Models;

namespace React_Redux_ASPdotNET_API.Server.Controllers
{
    /// <summary>
    /// Product controller for CRUD operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">ApplicationDbContext</param>
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>Complete list of prodcts from DB</returns>
        /// <responnse code="200">List of products</responnse>
        /// <responnse code="401">Un-Authorized request</responnse>
        [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// Get a product by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a product by ID</returns>
        /// <response code="200">Returns the product</response>
        /// <response code="404">If the product is not found</response>
        /// <responnse code="401">Un-Authorized request</responnse>
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Product created</returns>
        /// <response code="201">Returns the newly created product</response>
        /// <response code="400">If product already exist or data not correct</response>
        /// <responnse code="401">Un-Authorized request</responnse>
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [HttpPost("create")]
        public async Task<ActionResult<Product>> PostProduct([FromBody]Product product)
        {
            if (product == null) {
                return BadRequest("Product data not found!");
            }

            var exists = await _context.Products.FindAsync(product.Id);
            if (exists != null)
            {
                return BadRequest("Product already exists");
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
    }
}
