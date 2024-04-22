using Microsoft.AspNetCore.Mvc;
using CatalogService.Services;
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

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var searchResponse = await _client.SearchAsync<Product>(s => s
                .Index("product-logs")
                .Query(q => q
                    .MatchAll()
                )
            );

            if (searchResponse.IsValid)
            {
                return searchResponse.Documents;
            }
            else
            {
                throw new Exception("Failed to fetch documents from Elasticsearch", searchResponse.OriginalException);
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
