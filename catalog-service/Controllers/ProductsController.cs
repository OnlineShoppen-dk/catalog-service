using Microsoft.AspNetCore.Mvc;
using CatalogService.Services;
using CatalogService.Models;
using System.Threading.Tasks;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var documents = await _productService.GetAllProductsAsync();
                return Ok(documents);
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, "An error occurred while fetching documents from Elasticsearch");
            }
        }
        // GET: Products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            
            var product = await _productService.GetProductAsync(id);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound("Product not found.");
        }

        
    }
}
