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
