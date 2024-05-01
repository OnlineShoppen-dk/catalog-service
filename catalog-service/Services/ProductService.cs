using Elastic.Clients.Elasticsearch;
using CatalogService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
// using Nest;  // NEST Elasticsearch client library

namespace CatalogService.Services
{
    public class ProductService
    {
        private readonly ElasticsearchClient _client;

        public ProductService(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string query)
        {
            var response = await _client.SearchAsync<Product>(s => s 
                .Index("product-logs") 
                .From(0)
                .Size(10)
                .Query(q => q
                    .Wildcard(w => w
                        .Field(f => f.Name)
                        .Value($"*{query}*")
                        .CaseInsensitive(true)
                    )
                )
            );

            if (response.IsValidResponse)
            {
                return response.Documents; 
            }
            else
            {
                throw new Exception("Search failed or no results found");
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {

            var response = await _client.SearchAsync<Product>(s => s
                .Size(1000)
                .Query(q => 
                    q.Exists(t => t.Field(f => f.Id))
                )
            );
            if (response.Documents.Any())
            {
                return response.Documents;
            }
            else
            {
                throw new Exception("Failed to fetch documents from Elasticsearch");
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