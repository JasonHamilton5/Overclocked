using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using Xunit;

namespace Overclocked.Tests.PerformanceTests
{
    public class LoadTests
    {
        private const string BaseUrl = "http://localhost:5256";

        [Fact]
        public void GetProducts_Should_Handle_Load()
        {
            using var httpClient = new HttpClient();

            var scenario = Scenario.Create("get_products", async context =>
            {
                var request = Http.CreateRequest("GET", $"{BaseUrl}/api/products/GetProducts");
                var response = await Http.Send(httpClient, request);
                return response;
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.Inject(rate: 10, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(30))
            );

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();
        }

        [Fact]
        public void GetFeaturedProducts_Should_Handle_Load()
        {
            using var httpClient = new HttpClient();

            var scenario = Scenario.Create("get_featured_products", async context =>
            {
                var request = Http.CreateRequest("GET", $"{BaseUrl}/api/products/GetFeaturedProducts");
                var response = await Http.Send(httpClient, request);
                return response;
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.Inject(rate: 10, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(30))
            );

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();
        }

        [Fact]
        public void GetProductById_Should_Handle_Load()
        {
            using var httpClient = new HttpClient();

            var scenario = Scenario.Create("get_product_by_id", async context =>
            {
                var request = Http.CreateRequest("GET", $"{BaseUrl}/api/products/GetProducts/1");
                var response = await Http.Send(httpClient, request);
                return response;
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.Inject(rate: 10, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(30))
            );

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();
        }
    }
}