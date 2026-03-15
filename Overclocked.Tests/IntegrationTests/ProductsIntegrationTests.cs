using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Overclocked.API.Data;
using Overclocked.API.Models;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Overclocked.Tests.IntegrationTests
{
    public class ProductsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProductsIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove real database
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    // Add in-memory database
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase("TestDb"));

                    // Seed test data
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();

                    if (!db.Products.Any())
                    {
                        db.Products.AddRange(
                            new Product { Name = "Test Mouse", SKU = "TST-001", Price = 999.99m, Qty = 10, Category = "Mice" },
                            new Product { Name = "Test Keyboard", SKU = "TST-002", Price = 1499.99m, Qty = 20, Category = "Keyboards" }
                        );
                        db.SaveChanges();
                    }
                });
            });
        }

        [Fact]
        public async Task GetProducts_Returns_200_OK()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/products/GetProducts");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetProducts_Returns_List_Of_Products()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/products/GetProducts");
            var products = await response.Content.ReadFromJsonAsync<List<Product>>();
            Assert.NotNull(products);
            Assert.True(products.Count > 0);
        }

        [Fact]
        public async Task GetProduct_By_Valid_Id_Returns_200_OK()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/products/GetProducts/1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetProduct_By_Invalid_Id_Returns_404()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/products/GetProducts/99999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetProducts_Returns_Correct_Product_Data()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/products/GetProducts/1");
            var product = await response.Content.ReadFromJsonAsync<Product>();
            Assert.NotNull(product);
            Assert.Equal("TST-001", product.SKU);
        }
    }
}