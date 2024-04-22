using Elastic.Clients.Elasticsearch;
using CatalogService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;  // NEST Elasticsearch client library

namespace CatalogService.Services
{
    public class ProductService
    {
        private readonly ElasticsearchClient _client;

        public ProductService(ElasticsearchClient client)
        {
            _client = client;
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

        public async Task<Product> GetProductAsync(int productId)
        {
            var searchResponse = await _client.SearchAsync<Product>(s => s
                .Index("product-logs")
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Id)
                        .Query(productId.ToString())
                    )
                )
            );

            if (searchResponse.Documents.Any())
            {
                return searchResponse.Documents.First();
            }
            return null; // Document not found or other issue
        }
    }
}
