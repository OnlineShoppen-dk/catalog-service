using System;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using CatalogService.Services;
using System;
using System.Text.Json;
using Elastic.Clients.Elasticsearch.QueryDsl;

var builder = WebApplication.CreateBuilder(args);

// Configure Elasticsearch client with API Key and Certificate Fingerprint
var settings = new ElasticsearchClientSettings(new Uri("http://localhost:9200"))
    .Authentication(new BasicAuthentication("elastic", "changeme"))
    .EnableDebugMode()  // Optional: Enables detailed logging for debugging
    .PrettyJson()       // Optional: Formats JSON output to be more readable
    .RequestTimeout(TimeSpan.FromMinutes(2));  // Sets a timeout for requests


var client = new ElasticsearchClient(settings);
builder.Services.AddSingleton(client);
builder.Services.AddSingleton<ProductService>();
builder.Services.AddControllers();


// Continue setting up the web application
var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Maps attribute-routed controllers.
});

// Configure the HTTP request pipeline if needed
// For example:
// app.UseHttpsRedirection();

app.Run();
