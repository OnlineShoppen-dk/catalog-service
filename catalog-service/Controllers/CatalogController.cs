using Microsoft.AspNetCore.Mvc;
using CatalogService.Services;
using CatalogService.Models;
using System.Threading.Tasks;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, "An error occurred while fetching documents from Elasticsearch error: ${ex.Message}");
            }
        }

        // GET: /Product/search?query=some-text
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            try
            {
                var products = await _productService.SearchProductsAsync(query);
                if (products.Any())
                {
                    return Ok(products);
                }
                return NotFound("No products found matching your query.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while performing the search: {ex.Message}");
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

        // [HttpGet("search")]
        // public async Task<IActionResult> Search(string query)
        // {
        //     try
        //     {
        //         var products = await _productService.SearchProductsAsync(query);
        //         if (products.Any())
        //         {
        //             return Ok(products);
        //         }
        //         return NotFound("No products found matching your query.");
        //     }
        //     catch (Exception ex)
        //     {
        //         // Log the exception here
        //         return StatusCode(500, "An error occurred while performing the search");
        //     }
        // }

        
    }
}
